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

        private Sensor GetSensorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Sensors where c.UUID == uuid select c).Single(); ;
        }
        private void CreateNewSensorData(string value, DateTime loggedAt, Sensor sensor)
        {
            SensorData data = new SensorData();

            data.Value = value;
            data.LoggedAt = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            _wmsdbcontext.SensorDatas.Add(data);

            UpdateLastDataOfSensor(value.ToString(), loggedAt, sensor);
            UpdateCumulativeDataOfSensor(value, sensor);                    
        }
        private void UpdateCumulativeDataOfSensor(string value, Sensor sensor)
        {
            if (sensor is EnergySensor)
            {
                ((EnergySensor)sensor).CumulativeValue = (Convert.ToDecimal(((EnergySensor)sensor).CumulativeValue)+ Convert.ToDecimal(value)).ToString();
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
