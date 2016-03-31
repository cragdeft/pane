using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Areas
{
    [NotMapped]
    public class ZoneGoogleMap
    {
        public virtual IList<Zone> Zones { get; set; }
    }
}
