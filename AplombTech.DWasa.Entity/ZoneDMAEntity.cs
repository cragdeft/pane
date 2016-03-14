using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class ZoneDMAEntity
    {
        public List<ZoneEntity> ZoneList { get; set; }
        public DMAEntity DmaEntity { get; set; } 
    }
}
