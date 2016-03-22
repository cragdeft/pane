using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    [Table("Routers")]
    public class Router:Device
    {
        public string MacId { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
