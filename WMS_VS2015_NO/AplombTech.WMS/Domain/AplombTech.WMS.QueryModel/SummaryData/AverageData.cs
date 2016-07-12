using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.SummaryData
{
    public class AverageData
    {
        #region Primitive Properties
        [Key]
        public virtual Int64 AverageDataId { get; set; }
        public virtual DateTime DataDate { get; set; }
        public virtual DateTime ProcessAt { get; set; }
        #endregion
    }
}
