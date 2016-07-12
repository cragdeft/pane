using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Motors
{
    public class MotorData
    {
        #region Primitive Properties
        public virtual Int64 MotorDataID { get; set; }
        public virtual string MotorStatus { get; set; }
        public virtual string LastCommand { get; set; }
        public virtual string LastCommandTime { get; set; }
        public virtual bool Auto { get; set; }
        public virtual DateTime LoggedAt { get; set; }
        public virtual DateTime ProcessAt { get; set; }

        #endregion

        #region  Navigation Properties
        public virtual Motor Motor { get; set; }
        #endregion
    }
}
