using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    [Table("Pumps")]
    public class Pump : Device
    {
        public string UId { get; set; }
    }
}
