using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.SummaryData
{
    public class SensorSummaryDataHourly
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int SensorHourlyDataId { get; set; }
        public virtual DateTime DataDate { get; set; }
        public virtual int DataHour { get; set; }
        public virtual decimal DataValue { get; set; }
        public virtual DateTime ProcessAt { get; set; }

        #endregion

        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
