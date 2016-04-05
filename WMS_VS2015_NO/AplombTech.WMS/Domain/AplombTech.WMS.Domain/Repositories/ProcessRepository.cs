using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Repositories
{
    public class ProcessRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion
        public void ParseNStoreSensorData(SensorDataLog dataLog)
        {
            if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
            {
                try
                {
                    SensorMessage messageObject = JsonManager.GetSensorObject(dataLog.Message);

                    foreach (SensorValue data in messageObject.Sensors)
                    {
                        Sensor sensor = AreaRepository.FindSensorByUid(data.SensorUUID);
                        CreateNewSensorData(Convert.ToDecimal(data.Value), (DateTime)messageObject.SensorLoggedAt, sensor);
                    }

                    dataLog.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Done;
                }
                catch (Exception e)
                {
                    dataLog.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Failed;
                }
            }
        }

        public SensorDataLog LogSensorData(string topic, string message)
        {
            DateTime? LoggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
            int? pumpStationId = JsonManager.GetSensorPumpStationID(message);

            if (LoggedAtTime == null || pumpStationId == null) return null;

            SensorDataLog sensorLogData = GetSensorLogData(topic, (DateTime)LoggedAtTime, (int)pumpStationId);

            if (sensorLogData == null)
            {
                SensorDataLog data = CreateLog(topic, message, (DateTime)LoggedAtTime, (int)pumpStationId);
                return data;
            }

            return sensorLogData;
        }

        private SensorDataLog GetSensorLogData(string topic, DateTime loggedAtSensor, int stationId)
        {
            SensorDataLog dataLog = Container.Instances<SensorDataLog>().Where(w => w.PumpStation.AreaID == stationId && w.Topic == topic && w.LoggedAtSensor == loggedAtSensor).FirstOrDefault();

            return dataLog;
        }

        private SensorDataLog CreateLog(string topic, string message, DateTime loggedAtSensor, int stationId)
        {
            SensorDataLog data = Container.NewTransientInstance<SensorDataLog>();

            data.Topic = topic;
            data.Message = message;
            data.MessageReceivedAt = DateTime.Now;
            data.LoggedAtSensor = loggedAtSensor;
            data.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.None;
            data.PumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == stationId).First();

            Container.Persist(ref data);

            return data;
        }

        private void CreateNewSensorData(decimal value, DateTime loggedAt, Sensor sensor)
        {
            SensorData data = Container.NewTransientInstance<SensorData>();

            data.Value = value;
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;

            Container.Persist(ref data);
        }
    }
}
