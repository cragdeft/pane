using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
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
    public class DataProcessor : IHandleMessages<ProcessSensorData>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum JsonMessageType
        {
            configuration,
            sensordata,
            feedback
        }
        public void Handle(ProcessSensorData message)
        {
            log.Info("Sensor Data process has started for Id : " + message.SensorDataLogId);
            using (WMSUnitOfWork uow = new WMSUnitOfWork(false))
            {
                ProcessRepository repo = new ProcessRepository(WMSUnitOfWork.CurrentObjectContext);
                DataLog dataLog = repo.GetDataLogById(message.SensorDataLogId);

                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.sensordata.ToString())
                {
                    repo.ParseNStoreSensorData(dataLog);
                }
                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.configuration.ToString())
                {
                    //repo.ParseNStoreConfigurationData(dataLog);
                }
                log.Info("Sensor Data process has ended for Id : " + message.SensorDataLogId);
            }         
        }
    }
}
