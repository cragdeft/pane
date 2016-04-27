using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Messages.Commands;
using NakedObjects;
using NakedObjects.Services;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public class DataProcessor : AbstractFactoryAndRepository, IHandleMessages<ProcessSensorData>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Injected Services
        public INakedObjectsFramework NoFramework { set; protected get; }
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        #endregion

        public enum JsonMessageType
        {
            configuration,
            sensordata,
            feedback
        }
        public void Handle(ProcessSensorData message)
        {
            log.Info("Sensor Data process has started for Id : " + message.SensorDataLogId);
            DataLog dataLog = null;
            try
            {
                NoFramework.TransactionManager.StartTransaction();
                dataLog = ProcessRepository.GetDataLogById(message.SensorDataLogId);

                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.sensordata.ToString())
                {
                    ProcessRepository.ParseNStoreSensorData(dataLog);
                }
                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.configuration.ToString())
                {
                    ProcessRepository.ParseNStoreConfigurationData(dataLog);
                }
                NoFramework.TransactionManager.EndTransaction();
                log.Info("Sensor Data process has ended for Id : " + message.SensorDataLogId);
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in Sensor Data Process for Id : " + message.SensorDataLogId + ". Error: " + ex.ToString());
                NoFramework.TransactionManager.AbortTransaction();
                NoFramework.TransactionManager.StartTransaction();
                dataLog.ProcessingStatus = DataLog.ProcessingStatusEnum.Failed;
                dataLog.Remarks = "Error Occured in ProcessMessage method. Error: " + ex.ToString();
                NoFramework.TransactionManager.EndTransaction();
            }
        }
    }
}
