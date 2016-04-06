using AplombTech.WMS.Domain.Areas;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    public class SensorDataLog
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int SensorDataLogID { get; set; }
        [MemberOrder(10), Required]
        public virtual string Topic { get; set; }
        [MemberOrder(20), Required]
        public virtual string Message { get; set; }
        [MemberOrder(30), Required]
        public virtual DateTime MessageReceivedAt { get; set; }
        [MemberOrder(30), Required]
        public virtual DateTime LoggedAtSensor { get; set; }
        [MemberOrder(40), Required]
        public virtual ProcessingStatusEnum ProcessingStatus { get; set; }
        public enum ProcessingStatusEnum
        {
            None = 0,
            Done = 1,
            Started = 2,
            Failed = 3
        }

        #endregion

        #region  Navigation Properties
        [MemberOrder(60)]
        public virtual PumpStation PumpStation { get; set; }
        #endregion
    }
}
