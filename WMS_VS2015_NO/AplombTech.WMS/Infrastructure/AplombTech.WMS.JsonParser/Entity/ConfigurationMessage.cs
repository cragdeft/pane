using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Sensors;

namespace AplombTech.WMS.JsonParser.Entity
{
    public class ConfigurationMessage
    {
        public ConfigurationMessage()
        {
            Cameras = new List<Camera>();
            Sensors = new List<Sensor>();
            Router = new Router();
            Pump = new Pump();
        }
        public int? PumpStationId { get; set; }
        public DateTime? ConfigurationLoggedAt { get; set; }

        public IList<Camera> Cameras { get; set; }

        public Router Router { get; set; }

        public Pump Pump { get; set; }

        public IList<Sensor> Sensors { get; set; }
    }
}
