using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NakedObjects;
using AplombTech.WMS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AplombTech.WMS.Domain.Repositories;

namespace AplombTech.WMS.Domain.Areas
{
    public class Area
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        public AreaRepository AreaRepository { set; protected get; }
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
        public virtual int AreaID { get; set; }
        [Title]
        [MemberOrder(10)]
        public virtual string Name { get; set; }

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
        //[Required, NakedObjectsIgnore]
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

        #region  Navigation Properties
        [MemberOrder(30)]
        public virtual Area Parent { get; set; }
        public bool HideParent()
        {
            if (this.Parent != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [MemberOrder(40), Disabled]
        public virtual Address Address { get; set; }
        //[PageSize(10)]
        //public IQueryable<DMA> AutoCompleteAddress([MinLength(3)] string name)
        //{
        //    return AreaRepository.FindDMA(name);
        //}
        #endregion
    }
}
