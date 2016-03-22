using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class RouterEntity: DeviceEntity
    {
        public string MacId { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
