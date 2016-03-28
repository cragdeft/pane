using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class PumpEntity : DeviceEntity
    {
        [JsonProperty("uid")]
        public string UId { get; set; }
    }
}
