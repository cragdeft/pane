using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Motors
{
    public class ChlorineMotor : Motor
    {
        [StringLength(250)]
        public virtual string RemoveRemarks { get; set; }
        public virtual bool IsRemoved { get; set; }
    }

}
