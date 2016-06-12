﻿using AplombTech.WMS.Domain.Areas;
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
using Camera = AplombTech.WMS.QueryModel.Devices.Camera;

namespace AplombTech.WMS.Domain.Repositories
{
    public class ProcessRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

        public void ParseNStoreConfigurationData(DataLog dataLog)
        {
            if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
            {
                ConfigurationMessage messageObject = JsonManager.GetConfigurationObject(dataLog.Message);

                AddCameras(messageObject);

                AddPumpMotor(messageObject);

                AddCholorineMotor(messageObject);

                //AddRouter(messageObject);

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

            else if (sensor is QueryModel.Sensors.ChlorinePresenceDetector)
                type = Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR;

            else if (sensor is QueryModel.Sensors.ACPresenceDetector)
                type = Sensor.TransmitterType.AC_PRESENCE_DETECTOR;

            else if (sensor is QueryModel.Sensors.BatteryVoltageDetector)
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
                data.Value = Convert.ToDecimal(data.Value);
            if (sensor.DataType == Sensor.Data_Type.Boolean)
                data.Value = Convert.ToDecimal(Convert.ToBoolean(data.Value));
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            UpdateLastDataOfSensor(data.Value, loggedAt, sensor);
            UpdateCumulativeDataOfSensor(data.Value, sensor);
          
            Container.Persist(ref data);            
        }
        public void CreateNewMotorData(MotorValue data, DateTime loggedAt, Motor motor)
        {
            MotorData motorData = Container.NewTransientInstance<MotorData>();
            motorData.MotorStatus = data.MotorStatus;
            motorData.LastCommand = data.LastCommand;
            motorData.LastCommandTime = data.LastCommandTime;
            motorData.LoggedAt = loggedAt;
            motorData.ProcessAt = DateTime.Now;
            motorData.Motor = motor;
            Container.Persist(ref motorData);

            UpdateLastDataOfMotor(data, loggedAt, motor);
        }
        private void UpdateLastDataOfMotor(MotorValue data, DateTime loggedAt, Motor motor)
        {
            motor.Auto = data.Auto;
            motor.Controllable = data.Controllable;
            motor.MotorStatus = data.MotorStatus;
        }
        private void UpdateCumulativeDataOfSensor(decimal value, Sensor sensor)
        {
            if (sensor is EnergySensor)
            {
                ((EnergySensor)sensor).CumulativeValue += value;
            }
            if (sensor is FlowSensor)
            {
                ((FlowSensor)sensor).CumulativeValue += value;
            }
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
    }
}
