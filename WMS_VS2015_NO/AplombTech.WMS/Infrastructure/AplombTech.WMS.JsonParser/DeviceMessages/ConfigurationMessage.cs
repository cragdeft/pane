using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Motors;
using AplombTech.WMS.QueryModel.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages
{
    public class ConfigurationMessage : DeviceMessage
    {
        public ConfigurationMessage()
        {
            Cameras = new List<Camera>();
            Sensors = new List<Sensor>();
            Router = new Router();
            PumpMotor = new PumpMotor();
            ChlorineMotor = new ChlorineMotor();
        }
        public IList<Camera> Cameras { get; set; }
        public Router Router { get; set; }
        public PumpMotor PumpMotor { get; set; }
        public ChlorineMotor ChlorineMotor { get; set; }
        public IList<Sensor> Sensors { get; set; }
    }
}
