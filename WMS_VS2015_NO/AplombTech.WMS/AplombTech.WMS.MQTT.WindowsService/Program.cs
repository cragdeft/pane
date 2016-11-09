using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.WindowsService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string []args)
    {
            //if (Environment.UserInteractive)
            //{
            //  Service1 service1 = new Service1();
            //  service1.TestStartupAndStop(args);
            //}
            //else
            //{
            //  // Put the body of your old Main method here.
            //}

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
  }
}
