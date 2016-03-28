using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity.JsonCommandEntity
{
    public class ConfigurationCommandJsonEntity
    {
        [JsonProperty("PumoStation_Id")]
        public string PumoStationId { get; set; }
        public DateTime ConfigureDateTime { get; set; }
        public List<CameraEntity> Camera { get; set; }

        public PumpEntity Pump { get; set; }
        public List<RouterEntity> Router { get; set; }
        public List<SensorEntity> Sensor { get; set; }

    }

    public class ConfigurationJsonRootObject
    {
        [JsonProperty("PumpStation")]
        public ConfigurationCommandJsonEntity ConfigureDataJson { get; set; }
        

    }
}
