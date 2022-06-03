using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Port_COM_to_LAN
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent(); 
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                new BancoPostGres();
                new PortaLANServer();
            }
            catch (Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }            
        }

        protected override void OnStop()
        {
            this.Dispose();
        }
    }
}
