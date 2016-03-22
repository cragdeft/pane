using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    [ComplexType]
    public class AuditFields
    {
        public AuditFields()
        {

        }

        public AuditFields(string insertedBy, DateTime? insertedDateTime, string lastUpdatedBy,
            DateTime? lastUpdatedDateTime)
        {
            this.LastUpdatedBy = lastUpdatedBy;
            this.InsertedBy = insertedBy;
            this.InsertedDateTime = insertedDateTime;
            this.LastUpdatedDateTime = lastUpdatedDateTime;
        }

        #region Primitive Properties
        [Required]
        public string InsertedBy { get; set; }
        [Required]
        public DateTime? InsertedDateTime { get; set; }
        [Required]
        public string LastUpdatedBy { get; set; }
        [Required]
        public DateTime? LastUpdatedDateTime { get; set; }

        #endregion
    }
}
