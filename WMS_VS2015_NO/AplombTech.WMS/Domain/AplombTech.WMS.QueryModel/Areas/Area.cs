using NakedObjects;
using AplombTech.WMS.QueryModel.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplombTech.WMS.QueryModel.Areas
{
    [Table("Areas")]
    public class Area
    {       
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int AreaId { get; set; }
        [Title]
        [MemberOrder(10)]
        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual string Location { get; set; }
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
