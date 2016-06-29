using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.SummaryData
{
    public class OnOffData
    {
        #region Primitive Properties
        [Key]
        public virtual int OnOffDataId { get; set; }
        public virtual DateTime OffDateTime { get; set; }
        public virtual DateTime? OnDateTime { get; set; }
        public virtual double Duration { get; set; }
        public virtual DateTime ProcessAt { get; set; }
        #endregion
    }
}
