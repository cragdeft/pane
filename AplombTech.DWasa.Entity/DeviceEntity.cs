using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AplombTech.DWasa.Entity
{
    public class DeviceEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }
        public string Value { get; set; }
        
        public  PumpStationEntity PumpStation { get; set; }
    }
}
