using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Shared
{
    [Table("Addresses")]
    public class Address
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int AddressID { get; set; }
        [StringLength(200)]
        public virtual string Street1 { get; set; }
        [StringLength(200)]
        public virtual string Street2 { get; set; }
        [StringLength(50)]
        public virtual string ZipCode { get; set; }
        [StringLength(50)]
        public virtual string Zone { get; set; }
        [StringLength(50)]
        public virtual string City { get; set; }
        #region Complex Properties
        #region AuditFields (AuditFields)

        private AuditFields _auditFields = new AuditFields();

        [MemberOrder(250)]
        [Required]
        public virtual AuditFields AuditFields
        {
            get
            {
                return _auditFields;
            }
            set
            {
                _auditFields = value;
            }
        }

        public bool HideAuditFields()
        {
            return true;
        }
        #endregion
        #endregion
        #endregion

    }
}
