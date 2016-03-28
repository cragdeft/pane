using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class CameraEntity : DeviceEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("uid")]
        public string UId { get; set; }
    }
}
