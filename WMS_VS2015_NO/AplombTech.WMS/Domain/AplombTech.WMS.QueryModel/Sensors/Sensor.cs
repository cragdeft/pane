using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Shared;
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
    public class Sensor
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int SensorID { get; set; }
        [MemberOrder(10), NakedObjectsIgnore]
        [StringLength(50)]
        public virtual string UUID { get; set; }
        [MemberOrder(40)]
        public virtual decimal MinimumValue { get; set; }
        [MemberOrder(50)]
        public virtual decimal MaximumValue { get; set; }
        [MemberOrder(20), Required, Disabled]
        public virtual string CurrentValue { get; set; }
        [MemberOrder(60), Disabled]
        public virtual DateTime? LastDataReceived { get; set; }
        [MemberOrder(70), Required, Disabled]
        public virtual bool IsActive { get; set; }
        public virtual Data_Type DataType { get; set; }
        public virtual string UnitName { get; set; }
        public virtual string Model { get; set; }
        public virtual string Version { get; set; }
        public enum TransmitterType
        {
            PRESSURE_TRANSMITTER = 1,
            CHLORINE_TRANSMITTER = 2,
            ENERGY_TRANSMITTER = 3,
            FLOW_TRANSMITTER = 4,
            LEVEL_TRANSMITTER = 5,
        }

        public enum Data_Type
        {
            Float = 1,
            Boolean = 2
        }
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
        [MemberOrder(100)]
        public virtual PumpStation PumpStation { get; set; }
        #endregion
    }
}
