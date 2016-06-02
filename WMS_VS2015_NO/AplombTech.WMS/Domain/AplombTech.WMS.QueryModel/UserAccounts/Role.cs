using AplombTech.WMS.QueryModel.Features;
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
        #region Features
        [MemberOrder(50), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Features")]
        [TableView(false, "FeatureName", "FeatureType")]
        public IList<Feature> Features
        {
            get
            {
                IList<Feature> features = (from r in Container.Instances<RoleFeatures>()
                                           where r.Role.Id == this.Id
                                           select r.Feature).OrderBy(o => o.FeatureName).ToList();
                return features;
            }
        }
        #endregion
        #endregion       
    }
}
