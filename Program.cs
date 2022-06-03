using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.Reflection;

namespace Port_COM_to_LAN
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
                                {
                              new Service1()
                                };
            ServiceBase.Run(servicesToRun);

        }
    }
}
