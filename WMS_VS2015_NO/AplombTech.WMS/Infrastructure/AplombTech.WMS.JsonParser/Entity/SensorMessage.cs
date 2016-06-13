using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
    public class SensorMessage
    {
        public SensorMessage()
        {
            Sensors = new List<SensorValue>();
            Motors = new List<MotorValue>();
        }
        public int PumpStationId { get; set; }
        public DateTime SensorLoggedAt { get; set; }
        public bool SensorDataComplete { get; set; }
        public int LogCount { get; set; }
        public IList<SensorValue> Sensors { get; set; }
        public IList<MotorValue> Motors { get; set; }
    }
}
