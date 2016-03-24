using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Devices
{
    [Table("Pumps")]
    public class Pump : Device
    {
        [Title]
        [MemberOrder(20)]
        public virtual string ModelNo { get; set; }
    }
}
