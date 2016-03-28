using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class SensorEntity:DeviceEntity
    {
        [JsonProperty("uid")]
        public string UId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        public string Unit { get; set; }
    }
}
