using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Model.Enums;
using AplombTech.DWasa.Model.Models;

namespace AplombTech.DWasa.Model.Models
{
    [Table("Sensors")]
    public abstract class Sensor:Device
    {
        public string Unit { get; set; }
        public string UId { get; set; }
    }
}
