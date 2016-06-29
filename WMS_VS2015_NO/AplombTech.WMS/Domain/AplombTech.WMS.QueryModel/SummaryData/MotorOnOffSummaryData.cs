
using AplombTech.WMS.QueryModel.Motors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.SummaryData
{
    [Table("MotorOnOffSummaryData")]
    public class MotorOnOffSummaryData : OnOffData
    {
        #region  Navigation Properties
        public virtual Motor Motor { get; set; }
        #endregion
    }
}
