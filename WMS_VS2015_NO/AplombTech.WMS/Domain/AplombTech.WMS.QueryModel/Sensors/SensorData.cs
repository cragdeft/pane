using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Sensors
{
    public class SensorData
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int SensorDataID { get; set; }
        [MemberOrder(10), Required]
        public virtual decimal Value { get; set; }
        [MemberOrder(30), Required]
        public virtual DateTime LoggedAt { get; set; }

        #endregion

        #region  Navigation Properties
        [MemberOrder(40), Required]
        public virtual Sensor Sensor { get; set; }       
        #endregion
    }
}
