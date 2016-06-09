using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Entity;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Devices;
using Camera = AplombTech.WMS.QueryModel.Devices.Camera;

namespace AplombTech.WMS.Domain.Repositories
{
    public class ProcessRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

        public IList<DataLog> GetUnprocessedData()
        {
            return Container.Instances<DataLog>().Where(w => w.ProcessingStatus == DataLog.ProcessingStatusEnum.None).OrderBy(o=>o.LoggedAtSensor).Take(10).ToList();
        }
        public DataLog GetDataLogById(int id)
        {
            return Container.Instances<DataLog>().Where(w => w.SensorDataLogID == id).FirstOrDefault();
        }
        public void ParseNStoreConfigurationData(DataLog dataLog)
        {
            if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
            {
                ConfigurationMessage messageObject = JsonManager.GetConfigurationObject(dataLog.Message);

                AddCameras(messageObject);

                AddPump(messageObject);

                AddRouter(messageObject);

                AddSensor(messageObject);

                dataLog.ProcessingStatus = DataLog.ProcessingStatusEnum.Done;
            }
        }
        private void AddCameras(ConfigurationMessage messageObject)
        {
            foreach (var camera in messageObject.Cameras)
            {
                AreaRepository.AddCamera((int) messageObject.PumpStationId, camera.UUID, camera.URL);
            }
        }
        private void AddPump(ConfigurationMessage messageObject)
        {
                AreaRepository.AddPump((int)messageObject.PumpStationId,messageObject.Pump.UUID,messageObject.Pump.ModelNo);
        }
        private void AddRouter(ConfigurationMessage messageObject)
        {
            AreaRepository.AddRouter((int)messageObject.PumpStationId, messageObject.Router.UUID, messageObject.Router.IP, messageObject.Router.Port);
        }
        private void AddSensor(ConfigurationMessage messageObject)
        {
            foreach (var sensor in messageObject.Sensors)
            {
                Sensor.TransmitterType type = Sensor.TransmitterType.FLOW_TRANSMITTER;
                type = GetSensorType(sensor, type);
                AreaRepository.AddSensor(messageObject.PumpStationId, sensor.UUID, sensor.MinimumValue,sensor.MaximumValue,type);
            }
        }
        private static Sensor.TransmitterType GetSensorType(QueryModel.Sensors.Sensor sensor, Sensor.TransmitterType type)
        {
            if (sensor is WMS.QueryModel.Sensors.FlowSensor)
                type = Sensor.TransmitterType.FLOW_TRANSMITTER;
            else if (sensor is QueryModel.Sensors.PressureSensor)
                type = Sensor.TransmitterType.PRESSURE_TRANSMITTER;

            else if (sensor is QueryModel.Sensors.EnergySensor)
                type = Sensor.TransmitterType.ENERGY_TRANSMITTER;

            else if (sensor is QueryModel.Sensors.LevelSensor)
                type = Sensor.TransmitterType.LEVEL_TRANSMITTER;

            else if (sensor is QueryModel.Sensors.ChlorinationSensor)
                type = Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR;
            return type;
        }
        public DataLog LogData(string topic, string message)
        {
            DateTime loggedAtTime = DateTime.MinValue;
            int pumpStationId = 0;

            try
            {
                if (topic == "/sensordata")
                    loggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
                if (topic == "/configuration")
                    loggedAtTime = JsonManager.GetConfigurationLoggedAtTime(message);

                pumpStationId = JsonManager.GetPumpStationIDFromJson(message);
            }
            catch (Exception)
            {               
                return null;
            }
            
            DataLog sensorLogData = GetDataLog(topic, loggedAtTime, pumpStationId);

            if (sensorLogData == null)
            {
                DataLog data = CreateDataLog(topic, message, loggedAtTime, pumpStationId);
                return data;
            }

            return sensorLogData;
        }
        private DataLog GetDataLog(string topic, DateTime loggedAtSensor, int stationId)
        {
            DataLog dataLog = Container.Instances<DataLog>().Where(w => w.PumpStation.AreaId == stationId && w.Topic == topic && w.LoggedAtSensor == loggedAtSensor).FirstOrDefault();

            return dataLog;
        }
        public DataLog CreateDataLog(string topic, string message, DateTime loggedAtSensor, int stationId)
        {
            DataLog data = Container.NewTransientInstance<DataLog>();
            PumpStation station = Container.Instances<PumpStation>().Where(w => w.AreaId == stationId).First();

            data.Topic = topic;
            data.Message = message;
            data.MessageReceivedAt = DateTime.Now;
            data.LoggedAtSensor = loggedAtSensor;
            data.ProcessingStatus = DataLog.ProcessingStatusEnum.None;
            data.PumpStation = station;

            Container.Persist(ref data);

            return data;
        }
        public void CreateNewSensorData(string value, DateTime loggedAt, Sensor sensor)
        {           
            SensorData data = Container.NewTransientInstance<SensorData>();

            data.Value = value;
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            UpdateLastDataOfSensor(value, loggedAt, sensor);
            UpdateCumulativeDataOfSensor(value, sensor);
          
            Container.Persist(ref data);            
        }
        private void UpdateCumulativeDataOfSensor(String value, Sensor sensor)
        {
            if (sensor is EnergySensor)
            {
                ((EnergySensor)sensor).CumulativeValue += (Convert.ToDecimal(((EnergySensor)sensor).CumulativeValue) + Convert.ToDecimal(value)).ToString(); ;
            }
            if (sensor is FlowSensor)
            {
                ((FlowSensor)sensor).CumulativeValue += (Convert.ToDecimal(((FlowSensor)sensor).CumulativeValue) + Convert.ToDecimal(value)).ToString(); ;
            }
        }
        private void UpdateLastDataOfSensor(string value, DateTime loggedAt, Sensor sensor)
        {
            if (sensor.LastDataReceived != null)
            {
                if (sensor.LastDataReceived < loggedAt)
                {
                    sensor.CurrentValue = value;
                    sensor.LastDataReceived = loggedAt;
                }
            }
            else
            {
                sensor.CurrentValue = value;
                sensor.LastDataReceived = loggedAt;
            }
        }
    }
}
