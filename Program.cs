using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace keepAliveServ
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            keepAliveService service = new keepAliveService();
            if (Environment.UserInteractive)
            {
                service.RunAsConsole(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
