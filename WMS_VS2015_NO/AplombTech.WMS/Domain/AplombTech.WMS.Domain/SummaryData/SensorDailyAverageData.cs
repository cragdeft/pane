﻿using AplombTech.WMS.Domain.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AplombTech.WMS.Domain.SummaryData
{
    [Table("SensorDailyAverageData")]
    public class SensorDailyAverageData : AverageData
    {
        #region Primitive Properties
        public virtual decimal DataValue { get; set; }
        public virtual int DataCount { get; set; }
        public virtual decimal AverageValue { get; set; }
        #endregion

        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
