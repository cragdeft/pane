using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    public class Sensor
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

        #region Life Cycle Methods
        // This region should contain any of the 'life cycle' convention methods (such as
        // Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

        public virtual void Persisting()
        {
            this.InsertedBy = Container.Principal.Identity.Name;
            this.InsertedDateTime = DateTime.Now;
            this.LastUpdatedBy = Container.Principal.Identity.Name;
            this.LastUpdatedDateTime = DateTime.Now;
        }
        public virtual void Updating()
        {
            this.LastUpdatedBy = Container.Principal.Identity.Name;
            this.LastUpdatedDateTime = DateTime.Now;
        }
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
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int SensorID { get; set; }
        [MemberOrder(10)]
        public virtual string UUID { get; set; }
        [MemberOrder(20)]
        public virtual decimal MinimumValue { get; set; }
        [MemberOrder(30)]
        public virtual decimal MaximumValue { get; set; }
        [DisplayName("SensorType"), MemberOrder(40), Required]
        public virtual TransmitterType SensorType { get; set; }

        public enum TransmitterType
        {
            PRESSURE_TRANSMITTER = 1,
            CHLORINE_TRANSMITTER = 2,
            ENERGY_TRANSMITTER = 3,
            FLOW_TRANSMITTER = 4,
            LEVEL_TRANSMITTER = 5,
        }

        #region InsertedBy (String)
        [MemberOrder(130)]
        [NakedObjectsIgnore, Required]
        [Column("InsertedBy")]
        public virtual string InsertedBy { get; set; }

        #endregion
        #region InsertedDateTime (DateTime)
        [MemberOrder(140), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("InsertedDate")]
        public virtual DateTime InsertedDateTime { get; set; }

        #endregion
        #region LastUpdatedBy (String)
        [MemberOrder(150)]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedBy")]
        public virtual string LastUpdatedBy { get; set; }

        #endregion
        #region LastUpdatedDateTime (DateTime)
        [MemberOrder(160), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedDate")]
        public virtual DateTime LastUpdatedDateTime { get; set; }

        #endregion

        #endregion

        //#region Complex Properties
        //#region AuditFields (AuditFields)

        //private AuditFields _auditFields = new AuditFields();

        //[MemberOrder(250)]
        //[Required, NakedObjectsIgnore]
        //public virtual AuditFields AuditFields
        //{
        //    get
        //    {
        //        return _auditFields;
        //    }
        //    set
        //    {
        //        _auditFields = value;
        //    }
        //}

        //#endregion
        //#endregion

        #region  Navigation Properties
        [MemberOrder(50)]
        public virtual PumpStation PumpStation { get; set; }

        [PageSize(10)]
        public IQueryable<PumpStation> AutoCompletePumpStation([MinLength(3)] string name)
        {
            return AreaRepository.FindPumpStation(name);
        }

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
