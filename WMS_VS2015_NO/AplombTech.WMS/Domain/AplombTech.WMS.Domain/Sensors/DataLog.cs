using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Entity;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("DataLogs")]
    public class DataLog
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        #endregion

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

        public void Process()
        {
            if (this.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
            {
                try
                {
                    //framework.TransactionManager.StartTransaction();
                    ProcessRepository.ParseNStoreSensorData(this);
                    //framework.TransactionManager.EndTransaction();
                }
                catch (Exception ex)
                {
                    //log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                    //framework.TransactionManager.AbortTransaction();
                    //framework.TransactionManager.StartTransaction();
                    this.ProcessingStatus = DataLog.ProcessingStatusEnum.Failed;
                    //framework.TransactionManager.EndTransaction();
                }
            }
        }
    }
}
