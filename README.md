# Tools

Powerinject.py - Python3 script to generate .PS1 payloads that perform process injection.

Powerhollow.py - Python3 script to generate .PS1 payloads that perform process hollowing with PPID spoofing.

Formatshellcode.py - Python3 script to format C# shellcode output by msfvenom into proper format for use with Builder.exe.

Port_ipeggs.py - Python3 script to format C# shellcode output by msfvenom into proper format for user with Builder.exe. FOR USE WITH CLI PAYLOADS!

Builder - C# project that compiles to Builder.exe which will craft different .exe/.dll payloads from Template.cs files in other projects.

Hollow - C# project that compiles to Hollow.exe which performs process hollowing with PPID spoofing.

Runnerinject - C# project that compiel to Runnerinject.exe which performs process injection.

clinject - C# project source code for (IP + port + process) cli passed process injection payload.

clhollow - C# project source code for (IP + port + PPID + process) cli passed process hollowing payload.

sql - C# project source code for SQL.exe project for exploitation of MSSQL servers in AD.

x64_met_staged_reversetcp_inject.exe - Command line args: IP PORT PROCESS_TO_INJECT(explorer)

x64_met_staged_reversetcp_hollow.exe - Command line args: IP PORT PROCESS_TO_HOLLOW(c:\\windows\\system32\\svchost.exe) PPID_SPOOF(explorer) 

sql.exe - x64 application for exploitation of linked SQL servers.  Has functionality for use with InstallUtil AppLocker bypass.

pscradle.docm - Word doc with caeser cipher encoding that calls powershell download cradle.  Use with vbobfuscate.ps1 to generate and replace obfuscated text in pscradle.docm.

vbobfuscate.ps1 - ps1 to generate caeser cipher code for pscradle.  Make sure offsets match for encrypt/decrypt. First output is download cradle, last is app name for app name check before running. 

clm-bypass.cs - C# code that can be comiled to bypass AMSI and run powershell commands.

# NOTES

With Powerinject/Powerhollow make sure you think about whether you will be calling PS download cradle from powershell or cmd.exe and use the appropriate mode when constructing payloads.  When you call powershell.exe <cradle> from cmd.exe or even from another powershell window, you are creating a child process and while the embedded AMSI bypass may work for the child process the parent process will detect the child performing malicious actions and flag it.
  
Do NOT use msfvenom encoders with any Hollowing tool. Causes problems.
  
Injection tools:
    Your target for injection must be of the same integrity or lower than the method by which you have code execution.  I.e. if you are running in medium integrity you cannot inject into spoolsv, inject into explorer.
  
Hollowing tools:
    Your target parent process for PPID spoofing must be of the same integrity or lower than the method by which you have code execution. I.e. if you are running in medium integrity you cannot specify spoolsv as the parent process.  Hollowed process will inherity integrity of parent process.
  
  On Word Macros:
  
  WordMacroRunner - This is a baseline runner that will return a shell from WINWORD.exe. Has capabilities to detect AMSI and patch it if found (for both 32bit and 64 bit) as well as contains shellcode for both 32bit and 64 bit Word so it can execute after detecting architecture. 
  
  WordMacroInject - This macro performs process injection.  Currently specified for explorer.exe. NOTE: This runner is really only good for 64-bit word.  Seeing as we have no idea what version of word an organization will be running, the use case for this is limited.  The issue stems from the fact that 32 bit processes cannot easily inject into 64 bit ones; The presumed typical target environment will be running 32 bit word on a 64 bit OS, which renders the injection into explorer impossibly.  There are advanced techniques out there that might be able to facilitate this (Heaven's gate) but no idea if they could be implemented in VBA. Additionally there is no telling what/if any other 32 bit processes suitable for injetion might be running on a target machine.  In theory code could be written to enumerate running 32 bit processes and then just try to inject into an arbitrary one, but there are obvious issues concerning stability, and longevity of the process to maintain a reverse shell.  In reality just using a non-injecting runner and then setting up a C2 to automigrate is probably best practice as they are equipped to do so.
  
  Setup/formatting information:
  1. Write "legitimate" contents of the word doc, select all, then navigate to Insert > Quick Parts > AutoTexts and Save Selection to AutoText Gallery
  2. Give it a name, make sure it's saved to that particular document and not a template. Hit ok. Then delete the content from the body of the word doc.
  3. Copy in/write your pretexting content to the body of the word doc.  This is the piece that include "enable macros, hit this key combo to execute" etc.
  4. Go to Macro's and click record new macro.  Ensure on both screens you select the current document and not a template.  Click keyboard and then hit a key combination to map (e.g. Alt + D).  Once you hit ok/close, recording will begin.  Then go click macros again, view, select the main runner sub, and then click run.  This will map that sequence to Alt + D so that when it is entered the runner sub will be executed.
  
  To DOs:
  Integrate Injection runner with baseline runner so that injection is performed if x64 word is detected.
  
  Discoveries:
  Latest patch defender (Oct 2021) seems to have an "AND" based signature for AutoOpen().  It can be used in macros for benign purposes but as soon as API calls are included (or at least things used in shellcode runners), it flags signature based detection.
  RtlMoveMemory API call is signatured.  Use RtlFillMemory instead. 
  Resolve Amsi.dll and the function calls within it either dynamically or heavily obfuscated when you go to patch it.
  Starting in Word 2019 the program is 64 bit by default. This means Word 2019,O365,2021 are all good candidates for Injection because Orgs/individuals would have to go out of their way to have downloaded the 32 bit one.
  Meterpreter shells after using Migrate seem to get caught by defender sometimes... Doesn't seem to be the case for straight up injection payloads.
  
 
