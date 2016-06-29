
using AplombTech.WMS.QueryModel.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.SummaryData
{
    [Table("SensorOnOffSummaryData")]
    public class SensorOnOffSummaryData : OnOffData
    {
        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
