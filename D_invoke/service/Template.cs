using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;

namespace service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
	<IMPORTS>
        protected override void OnStart(string[] args)
        {
		<MAIN>
        }
	<DECRYPTFUNC>
        protected override void OnStop()
        {
        }
    }
    <CLASSES>
}
