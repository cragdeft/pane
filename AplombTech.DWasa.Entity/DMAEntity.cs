using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class DMAEntity
    {
        #region Primitive Properties
        public int DMAId { get; set; }
        public string DMAName { get; set; }
        public string Address { get; set; }
        public string Locations { get; set; }
        #endregion
    }
}
