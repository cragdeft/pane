using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Devices
{
    [Table("Routers")]
    public class Router : Device
    {
        [MemberOrder(20)]
        public virtual string MACAddress { get; set; }
        [MemberOrder(30), Title]
        public virtual string IP { get; set; }
        [MemberOrder(40)]
        public virtual int Port { get; set; }
    }
}
