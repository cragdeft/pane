using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class DMAEntity:AreaEntity
    {
        #region Primitive Properties
        public ZoneEntity Zone { get; set; }
        #endregion
    }
}
