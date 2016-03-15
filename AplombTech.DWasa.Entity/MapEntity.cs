using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class MapEntity
    {
        public List<ZoneEntity> ZoneList { get; set; }
        public List<DMAEntity> DMAList { get; set; }
        public List<PumpStationEntity> PumpStationList { get; set; }
    }
}
