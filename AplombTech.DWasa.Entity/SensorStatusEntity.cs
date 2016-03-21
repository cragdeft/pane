using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class SensorStatusEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        
        #region  Navigation Properties
        public virtual DeviceEntity Device { get; set; }
        #endregion
    }
}
