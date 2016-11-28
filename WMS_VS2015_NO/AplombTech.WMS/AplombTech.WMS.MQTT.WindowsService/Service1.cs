using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.WindowsService
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

                KillOldProcess();
                Log.WriteLog("Going To Start Wasa MqTT");
                ProcessStartInfo info = new ProcessStartInfo(ConfigurationManager.AppSettings["MQTTConsole"].ToString());

                try
                {
                    Process.Start(info);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    Log.WriteLog(ex.Message);
                }
                Log.WriteLog(" Wasa MqTT Started");

                Log.WriteLog("Going To Start Wasa Data Processor");

                info = new ProcessStartInfo(ConfigurationManager.AppSettings["DataProcessConsole"].ToString());

                try
                {
                    Process.Start(info);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    Log.WriteLog(ex.Message);
                }
                Log.WriteLog("Wasa Data Processor started");
            }
            catch (Exception ex)
            {

                Log.WriteLog(ex.Message);
            }

        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        protected override void OnStop()
        {
            KillOldProcess();
        }

        private static void KillOldProcess()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("AplombTech.WMS.MQTT.Client.exe");
                foreach (var process in processes)
                {
                    process.Kill();
                }
                Log.WriteLog("Wasa MqTT stoped");
                processes = Process.GetProcessesByName("AplombTech.WMS.Sensor.Data.Processor.exe");
                foreach (var process in processes)
                {
                    process.Kill();
                }

                Log.WriteLog("Wasa Data Processor stopped");
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
    }
}
