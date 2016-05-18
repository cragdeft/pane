using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using NakedObjects;
using NakedObjects.Services;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                ProcessRepository repo = new ProcessRepository(WMSUnitOfWork.CurrentObjectContext);
                DataLog dataLog = repo.GetDataLogById(message.SensorDataLogId);

                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.sensordata.ToString())
                {
                    //repo.ParseNStoreSensorData(dataLog);
                }
                if (dataLog.Topic.Replace("/", String.Empty) == JsonMessageType.configuration.ToString())
                {
                    //repo.ParseNStoreConfigurationData(dataLog);
                }
                //try
                //{
                //    WMSUnitOfWork.CurrentObjectContext.SaveChanges();
                //}
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                //                        ve.PropertyName,
                //                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                //                        ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
                WMSUnitOfWork.CurrentObjectContext.SaveChanges();
                log.Info("Sensor Data process has ended for Id : " + message.SensorDataLogId);
            }         
        }
    }
}
