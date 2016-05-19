using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Features
{
    public class Feature
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int FeatureId { get; set; }
        [Title, Required]
        [MemberOrder(20)]
        [StringLength(100)]
        public virtual string FeatureName { get; set; }
        #endregion

        #region Navigation Properties
        [MemberOrder(10), Required, Disabled]
        public virtual FeatureType FeatureType { get; set; }
        #endregion
    }
}
