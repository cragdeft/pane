using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Devices
{
    [Table("Pumps")]
    public class Pump : Device
    {
        [Title]
        [MemberOrder(20)]
        public virtual string ModelNo { get; set; }
        public virtual decimal Capacity { get; set; }
        public virtual int StaticWaterLevel { get; set; }
        public virtual string RemoveRemarks { get; set; }
        public virtual bool IsRemoved { get; set; }
    }
}
