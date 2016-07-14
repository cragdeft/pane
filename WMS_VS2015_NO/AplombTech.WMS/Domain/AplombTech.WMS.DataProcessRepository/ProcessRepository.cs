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
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.AreaRepositories;

namespace AplombTech.WMS.DataProcessRepository
{
    public class ProcessRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

        public void StoreConfigurationData(ConfigurationMessage messageObject)
        {
            AddCameras(messageObject);
            AddPumpMotor(messageObject);
            AddCholorineMotor(messageObject);
            AddSensor(messageObject);
        }

        private void AddCameras(ConfigurationMessage messageObject)
        {
            foreach (var camera in messageObject.Cameras)
            {
                AreaRepository.AddCamera((int) messageObject.PumpStationId, camera.UUID, camera.URL);
            }
        }
        private void AddPumpMotor(ConfigurationMessage messageObject)
        {
                AreaRepository.AddPumpMotor((int)messageObject.PumpStationId,messageObject.PumpMotor.UUID,messageObject.PumpMotor.Auto, messageObject.PumpMotor.Controllable);
        }

        private void AddCholorineMotor(ConfigurationMessage messageObject)
        {
            AreaRepository.AddCholorineMotor((int)messageObject.PumpStationId, messageObject.PumpMotor.UUID, messageObject.PumpMotor.Auto, messageObject.PumpMotor.Controllable);
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
                AreaRepository.AddSensor(messageObject.PumpStationId, sensor.UUID, sensor.MinimumValue,sensor.MaximumValue,type,(Sensor.Data_Type) sensor.DataType,sensor.Model,sensor.Version,sensor.UnitName);
            }
        }
        private static Sensor.TransmitterType GetSensorType(Sensor sensor, Sensor.TransmitterType type)
        {
            if (sensor is FlowSensor)
                type = Sensor.TransmitterType.FLOW_TRANSMITTER;
            else if (sensor is PressureSensor)
                type = Sensor.TransmitterType.PRESSURE_TRANSMITTER;

            else if (sensor is EnergySensor)
                type = Sensor.TransmitterType.ENERGY_TRANSMITTER;

            else if (sensor is LevelSensor)
                type = Sensor.TransmitterType.LEVEL_TRANSMITTER;

            else if (sensor is ChlorinePresenceDetector)
                type = Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR;

            else if (sensor is ACPresenceDetector)
                type = Sensor.TransmitterType.AC_PRESENCE_DETECTOR;

            else if (sensor is BatteryVoltageDetector)
                type = Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR;
            return type;
        }
        public DataLog LogData(string topic, string message)
        {
            DateTime loggedAtTime = DateTime.MinValue;
            int pumpStationId = 0;

            try
            {
                if (topic == "wasa/sensor_data")
                    loggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
                if (topic == "wasa/configuration")
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
            if (sensor.DataType == Sensor.Data_Type.Float)
                data.Value = Convert.ToDecimal(value);
            if (sensor.DataType == Sensor.Data_Type.Boolean)
            {
                if (value != null && value.Contains("."))
                {
                    data.Value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(value)));
                }
                else
                {
                    data.Value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(value)));
                }
            }
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            if (sensor is FlowSensor || sensor is EnergySensor)
            {
                //DO NOTHING
            }            
            else
            {
                UpdateLastDataOfSensor(data.Value, data.ProcessAt, sensor);
            }
                     
            Container.Persist(ref data);            
        }        
        private void UpdateLastDataOfSensor(decimal value, DateTime loggedAt, Sensor sensor)
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
        public void CreateNewMotorData(MotorValue data, DateTime loggedAt, Motor motor)
        {
            MotorData motorData = Container.NewTransientInstance<MotorData>();
            motorData.MotorStatus = data.MotorStatus;
            motorData.LastCommand = data.LastCommand;
            motorData.LastCommandTime = data.LastCommandTime;
            motorData.Auto = data.Auto;
            motorData.LoggedAt = loggedAt;
            motorData.ProcessAt = DateTime.Now;
            motorData.Motor = motor;
            Container.Persist(ref motorData);

            UpdateLastDataOfMotor(data, motorData.ProcessAt, motor);
        }
        private void UpdateLastDataOfMotor(MotorValue data, DateTime loggedAt, Motor motor)
        {
            motor.Auto = data.Auto;
            motor.Controllable = data.Controllable;
            motor.MotorStatus = data.MotorStatus;
            motor.LastDataReceived = loggedAt;
        }        
    }
}
