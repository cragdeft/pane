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
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = GetSensorType();

            title = title + " - " + this.UUID;

            t.Append(title);
            
            return t.ToString();
        }

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int SensorID { get; set; }
        [MemberOrder(10), NakedObjectsIgnore]
        public virtual string UUID { get; set; }
        [MemberOrder(20)]
        public virtual decimal MinimumValue { get; set; }
        [MemberOrder(30)]
        public virtual decimal MaximumValue { get; set; }
        [DisplayName("SensorType"), MemberOrder(50), Required]
        public virtual TransmitterType SensorType { get; set; }

        public enum TransmitterType
        {
            PRESSURE_TRANSMITTER = 1,
            CHLORINE_TRANSMITTER = 2,
            ENERGY_TRANSMITTER = 3,
            FLOW_TRANSMITTER = 4,
            LEVEL_TRANSMITTER = 5,
        }
        #endregion

        #region Get Properties
        [MemberOrder(40), NotMapped]
        [DisplayName("Current Value")]
        public decimal CurrentValue
        {
            get
            {
                Decimal value = 0;

                SensorData sensordata = (from data in Container.Instances<SensorData>()
                                         where data.Sensor.SensorID == this.SensorID
                                         select data).OrderByDescending(o => o.LoggedAt).FirstOrDefault();
                if(sensordata != null)
                {
                    value = sensordata.Value;
                }
                return value;
            }
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
        [MemberOrder(60)]
        public virtual PumpStation PumpStation { get; set; }

        //[PageSize(10)]
        //public IQueryable<PumpStation> AutoCompletePumpStation([MinLength(3)] string name)
        //{
        //    return AreaRepository.FindPumpStation(name);
        //}

        #endregion

        private string GetSensorType()
        {
            string type = String.Empty;

            switch (this.SensorType)
            {
                case TransmitterType.CHLORINE_TRANSMITTER:
                    type = "CT";
                    break;

                case TransmitterType.ENERGY_TRANSMITTER:
                    type = "ET";
                    break;

                case TransmitterType.FLOW_TRANSMITTER:
                    type = "FT";
                    break;

                case TransmitterType.LEVEL_TRANSMITTER:
                    type = "LT";
                    break;

                case TransmitterType.PRESSURE_TRANSMITTER:
                    type = "PT";
                    break;
            }
            return type;
        }
    }
}
