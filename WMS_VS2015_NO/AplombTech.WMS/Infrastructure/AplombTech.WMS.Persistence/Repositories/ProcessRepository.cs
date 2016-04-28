﻿using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.Persistence.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Persistence.Repositories
{
    public class ProcessRepository
    {
        private readonly WMSDBContext _wmsdbcontext;

        public ProcessRepository(WMSDBContext wmsdbcontext)
        {
            _wmsdbcontext = wmsdbcontext;
        }

        public DataLog GetDataLogById(int id)
        {
            return (from c in _wmsdbcontext.SensorDataLogs where c.SensorDataLogID == id select c).Single(); ;
        }

        public void ParseNStoreSensorData(DataLog dataLog)
        {
            if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
            {
                SensorMessage messageObject = JsonManager.GetSensorObject(dataLog.Message);

                foreach (SensorValue data in messageObject.Sensors)
                {
                    Sensor sensor = GetSensorByUuid(data.SensorUUID);
                    CreateNewSensorData(Convert.ToDecimal(data.Value), (DateTime)messageObject.SensorLoggedAt, sensor);
                }

                dataLog.ProcessingStatus = DataLog.ProcessingStatusEnum.Done;
            }
        }

        private Sensor GetSensorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Sensors where c.UUID == uuid select c).Single(); ;
        }
        private void CreateNewSensorData(decimal value, DateTime loggedAt, Sensor sensor)
        {
            SensorData data = new SensorData();

            data.Value = value;
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            UpdateLastDataOfSensor(value, loggedAt, sensor);
            UpdateCumulativeDataOfSensor(value, sensor);

            _wmsdbcontext.SensorDatas.Add(data);
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
