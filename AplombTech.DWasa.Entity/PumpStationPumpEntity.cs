using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class PumpStationPumpEntity: PumpStationDeviceEntity
    {
        public PumpEntity Pump { get; set; }
    }
}
