﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class SensorStatusEntity
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime LogDateTime { get; set; }
        #region  Navigation Properties
        public virtual DeviceEntity Device { get; set; }
        #endregion
    }
}
