using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.SensorDataLogBoundedContext.Facade;

namespace AplombTech.WMS.SensorDataLogBoundedContext.Repositories
{
    public class SDLRepository
    {
        private readonly SensorDataLogContext _sensorDataLogContext;

        public SDLRepository(SensorDataLogContext sensorDataLogContext)
        {
            _sensorDataLogContext = sensorDataLogContext;
        }

        public DataLog GetDataLog(string topic, DateTime loggedAtSensor, int stationId)
        {
            return
                _sensorDataLogContext.DataLogs.Where(
                    w => w.PumpStationId == stationId && w.Topic == topic && w.LoggedAtSensor == loggedAtSensor)
                    .FirstOrDefault();
        }

        public DataLog CreateDataLog(string topic, string message, DateTime loggedAtSensor, int stationId)
        {
            DataLog data = new DataLog();

            data.Topic = topic;
            data.Message = message;
            data.MessageReceivedAt = DateTime.Now;
            data.LoggedAtSensor = loggedAtSensor;
            data.ProcessingStatus = DataLog.ProcessingStatusEnum.None;
            data.PumpStationId = stationId;
            _sensorDataLogContext.DataLogs.Add(data);
            return data;
        }

        public SensorData CreateNewSensorData(string value, DateTime loggedAt, Sensor sensor)
        {
            SensorData data = new SensorData();
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
            data.SensorId = sensor.SensorId;
            data.ProcessAt = DateTime.Now;

            _sensorDataLogContext.SensorDatas.Add(data);

            return data;
        }

        

        public SensorData GetSensorDataWithinTime(int SensorId, DateTime from, DateTime to)
        {
            SensorData sensorData = _sensorDataLogContext.SensorDatas
                  .Where(x => (x.SensorId == SensorId && x.LoggedAt >= from && x.LoggedAt <= to)).FirstOrDefault();

            return sensorData;

        }

        public void CreateNewMotorData(MotorData motorData)
        {
            _sensorDataLogContext.MotorDatas.Add(motorData);
        }
        
    }
}
