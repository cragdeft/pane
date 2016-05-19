using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.UserAccounts
{
    [Table("AspNetRoles")]
    [Bounded]
    public class Role
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual string Id { get; set; }
        [Title, Required]
        [MemberOrder(10)]
        public virtual string Name { get; set; }
        #endregion   

        #region Get Properties      
        [MemberOrder(70), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Users")]
        [TableView(false, "Email")]
        public IList<LoginUser> Users
        {
            get
            {
                IList<LoginUser> users = (from r in Container.Instances<UserRoles>()
                    where r.Role.Id == this.Id
                    select r.LoginUser).ToList();
                return users;
            }
        }
        #endregion
    }
}
