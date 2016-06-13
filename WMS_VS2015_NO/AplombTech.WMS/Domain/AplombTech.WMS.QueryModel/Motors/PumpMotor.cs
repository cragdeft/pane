using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NakedObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplombTech.WMS.QueryModel.Motors
{
    [Table("PumpMotors")]
    public class PumpMotor:Motor
    {
        [Title]
        [MemberOrder(20)]
        [StringLength(50)]
        public virtual string ModelNo { get; set; }
        public virtual decimal Capacity { get; set; }
        public virtual int StaticWaterLevel { get; set; }
    }
}
