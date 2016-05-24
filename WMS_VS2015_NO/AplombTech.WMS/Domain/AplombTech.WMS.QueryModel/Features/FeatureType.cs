using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Features
{
    public class FeatureType
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int FeatureTypeId { get; set; }
        [Title, Required]
        [MemberOrder(10)]
        [StringLength(50)]
        public virtual string FeatureTypeName { get; set; }
        #endregion

        #region FeatureType
        public enum FeatureTypeEnums
        {
            Area = 1,
            Alert = 2,
            UserAccount = 3,
            Report = 4
        }
        #endregion
    }
}
