using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Entity.JsonCommandEntity;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class SensorDataJsonEntity
    {
        [JsonProperty("PumoStation_Id")]
        public string PumoStationId { get; set; }
        public DateTime LogDateTime { get; set; }
        [JsonProperty("Sensor")]
        public List<SensorData> SensorData { get; set; }
    }

    public class SensorData
    {
        [JsonProperty("uid")]
        public string UId { get; set; }

        public string value { get; set; }
    }

    public class SensorJsonRootObject
    { 
        [JsonProperty("PumpStation")]
        public SensorDataJsonEntity SensorDataJson { get; set; }
    }
}
