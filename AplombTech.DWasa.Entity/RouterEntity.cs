using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class RouterEntity: DeviceEntity
    {
        [JsonProperty("mac_id")]
        public string MacId { get; set; }
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
