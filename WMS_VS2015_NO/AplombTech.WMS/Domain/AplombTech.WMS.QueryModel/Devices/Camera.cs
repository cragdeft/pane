using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Devices
{
    [Table("Cameras")]
    public class Camera : Device
    {
        [Title]
        [MemberOrder(20)]
        [StringLength(50)]
        public virtual string URL { get; set; }
    }
}
