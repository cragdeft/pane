using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Motors
{
    public class Motor
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int MotorID { get; set; }
        [MemberOrder(10), NakedObjectsIgnore]
        [StringLength(20)]
        public virtual string UUID { get; set; }
        public virtual bool Auto { get; set; }
        public virtual bool Controllable { get; set; }
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

        #region  Navigation Properties
        [MemberOrder(50)]
        public virtual PumpStation PumpStation { get; set; }
        #endregion

    }
}
