using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Shared
{
    [Bounded]
    public class Unit
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int UnitID { get; set; }
        [Title, DisplayName("Unit"), MemberOrder(10), Required]
        public virtual string Name { get; set; }
        #endregion

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
    }
}
