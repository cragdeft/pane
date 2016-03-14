using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class PumpStationEntity
    {
        #region Primitive Properties
        public int PumpId { get; set; }
        public string PumpName { get; set; }
        public string Locations { get; set; }
        public string Address { get; set; }
        #endregion
    }
}
