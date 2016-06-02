using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.UserAccounts
{
    [Table("AspNetUsers")]
    public class LoginUser
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual string Id { get; set; }
        [Title, Required, NakedObjectsIgnore]
        [MemberOrder(10)]
        public virtual string UserName { get; set; }
        [Required, MemberOrder(20), Disabled]
        public virtual string Email { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool EmailConfirmed { get; set; }
        [NakedObjectsIgnore]
        public virtual string PasswordHash { get; set; }
        [NakedObjectsIgnore]
        public virtual string SecurityStamp { get; set; }
        [NakedObjectsIgnore]
        public virtual string PhoneNumber { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool PhoneNumberConfirmed { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool TwoFactorEnabled { get; set; }
        [NakedObjectsIgnore]
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool LockoutEnabled { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual int AccessFailedCount { get; set; }
        #endregion

        #region Get Properties      
        [MemberOrder(50), NotMapped]
        [DisplayName("Role")]
        public Role Role
        {
            get
            {
                Role role = (from r in Container.Instances<UserRoles>()
                             where r.LoginUser.Id == this.Id
                             select r.Role).FirstOrDefault();
                return role;
            }
        }
        #endregion
    }
}
