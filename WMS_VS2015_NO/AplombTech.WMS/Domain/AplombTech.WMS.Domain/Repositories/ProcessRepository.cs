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

        public IList<SensorDataLog> GetUnprocessedData()
        {
            return Container.Instances<SensorDataLog>().Where(w => w.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None).OrderBy(o=>o.LoggedAtSensor).Take(10).ToList();
        }
        public SensorDataLog GetDataLogById(int id)
        {
            return Container.Instances<SensorDataLog>().Where(w => w.SensorDataLogID == id).FirstOrDefault();
        }
        public void ParseNStoreSensorData(SensorDataLog dataLog)
        {
            if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
            {
                    SensorMessage messageObject = JsonManager.GetSensorObject(dataLog.Message);

                    foreach (SensorValue data in messageObject.Sensors)
                    {
                        Sensor sensor = AreaRepository.FindSensorByUid(data.SensorUUID);
                        CreateNewSensorData(Convert.ToDecimal(data.Value), (DateTime)messageObject.SensorLoggedAt, sensor);
                    }

                    dataLog.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Done;
            }
        }

        public void ParseNStoreConfigurationData(SensorDataLog dataLog)
        {
            if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
            {
                ConfigurationMessage messageObject = JsonManager.GetConfigurationObject(dataLog.Message);

                AddCameras(messageObject);

                AddPump(messageObject);

                AddRouter(messageObject);

                AddSensor(messageObject);

                dataLog.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Done;
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

                if (sensor is WMS.QueryModel.Sensors.FlowSensor)
                    type = Sensor.TransmitterType.FLOW_TRANSMITTER;
                else if (sensor is QueryModel.Sensors.PressureSensor)
                    type = Sensor.TransmitterType.PRESSURE_TRANSMITTER;

                else if (sensor is QueryModel.Sensors.EnergySensor)
                    type = Sensor.TransmitterType.ENERGY_TRANSMITTER;

                else if (sensor is QueryModel.Sensors.LevelSensor)
                    type = Sensor.TransmitterType.LEVEL_TRANSMITTER;

                else if (sensor is QueryModel.Sensors.ChlorinationSensor)
                    type = Sensor.TransmitterType.CHLORINE_TRANSMITTER;

                AreaRepository.AddSensor((int)messageObject.PumpStationId, sensor.UUID, sensor.MinimumValue,sensor.MaximumValue,type);
            }
        }


        public SensorDataLog LogSensorData(string topic, string message)
        {
            DateTime? LoggedAtTime = DateTime.MinValue;

            if (topic == "/sensordata")
                LoggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
            if (topic == "/configuration")
                LoggedAtTime = JsonManager.GetConfigurationLoggedAtTime(message);

            int? pumpStationId = JsonManager.GetSensorPumpStationID(message);

            if (LoggedAtTime == null || pumpStationId == null) return null;

            SensorDataLog sensorLogData = GetSensorLogData(topic, (DateTime)LoggedAtTime, (int)pumpStationId);

            if (sensorLogData == null)
            {
                SensorDataLog data = CreateSensorDataLog(topic, message, (DateTime)LoggedAtTime, (int)pumpStationId);
                return data;
            }

            return sensorLogData;
        }

        private SensorDataLog GetSensorLogData(string topic, DateTime loggedAtSensor, int stationId)
        {
            SensorDataLog dataLog = Container.Instances<SensorDataLog>().Where(w => w.PumpStation.AreaID == stationId && w.Topic == topic && w.LoggedAtSensor == loggedAtSensor).FirstOrDefault();

            return dataLog;
        }

        public SensorDataLog CreateSensorDataLog(string topic, string message, DateTime loggedAtSensor, int stationId)
        {
            SensorDataLog data = Container.NewTransientInstance<SensorDataLog>();
            PumpStation station = Container.Instances<PumpStation>().Where(w => w.AreaID == stationId).First();

            data.Topic = topic;
            data.Message = message;
            data.MessageReceivedAt = DateTime.Now;
            data.LoggedAtSensor = loggedAtSensor;
            data.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.None;
            data.PumpStation = station;

            Container.Persist(ref data);

            return data;
        }

        public void CreateNewSensorData(decimal value, DateTime loggedAt, Sensor sensor)
        {           
            SensorData data = Container.NewTransientInstance<SensorData>();

            data.Value = value;
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            sensor.CurrentValue = value;
            if (sensor is EnergySensor )
            {
                ((EnergySensor)sensor).CumulativeValue += value;
            }
            if (sensor is FlowSensor)
            {
                ((FlowSensor)sensor).CumulativeValue += value;
            }

            Container.Persist(ref data);            
        }
    }
}
