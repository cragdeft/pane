using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Shared
{
    [Table("Addresses")]
    public class Address
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Life Cycle Methods
        // This region should contain any of the 'life cycle' convention methods (such as
        // Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

        public virtual void Persisting()
        {
            this.InsertedBy = Container.Principal.Identity.Name;
            this.InsertedDateTime = DateTime.Now;
            this.LastUpdatedBy = Container.Principal.Identity.Name;
            this.LastUpdatedDateTime = DateTime.Now;
        }
        public virtual void Updating()
        {
            this.LastUpdatedBy = Container.Principal.Identity.Name;
            this.LastUpdatedDateTime = DateTime.Now;
        }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int AddressID { get; set; }
        public virtual string Street1 { get; set; }
        public virtual string Street2 { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Zone { get; set; }
        public virtual string City { get; set; }
        #region InsertedBy (String)
        [MemberOrder(130)]
        [NakedObjectsIgnore, Required]
        [Column("InsertedBy")]
        public virtual string InsertedBy { get; set; }

        #endregion
        #region InsertedDateTime (DateTime)
        [MemberOrder(140), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("InsertedDate")]
        public virtual DateTime InsertedDateTime { get; set; }

        #endregion
        #region LastUpdatedBy (String)
        [MemberOrder(150)]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedBy")]
        public virtual string LastUpdatedBy { get; set; }

        #endregion
        #region LastUpdatedDateTime (DateTime)
        [MemberOrder(160), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedDate")]
        public virtual System.DateTime LastUpdatedDateTime { get; set; }

        #endregion
        #endregion

        //#region Complex Properties
        //#region AuditFields (AuditFields)

        //private AuditFields _auditFields = new AuditFields();

        //[MemberOrder(250)]
        //[Required]
        //public virtual AuditFields AuditFields
        //{
        //    get
        //    {
        //        return _auditFields;
        //    }
        //    set
        //    {
        //        _auditFields = value;
        //    }
        //}

        //#endregion
        //#endregion
    }
}
