using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class DeviceEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public  PumpStationEntity PumpStation { get; set; }
    }
}
