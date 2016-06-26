using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Motors;

namespace AplombTech.WMS.Persistence.Repositories
{
    public class ProcessRepository
    {
        private readonly WMSDBContext _wmsdbcontext;

        public ProcessRepository(WMSDBContext wmsdbcontext)
        {
            _wmsdbcontext = wmsdbcontext;
        }

        public void GenerateSummary(SummaryGenerationMessage message)
        {
            DateTime summaryDate = new DateTime(message.DataLoggedAt.Year, message.DataLoggedAt.Month,
                    message.DataLoggedAt.Day);

            int summaryHour = message.DataLoggedAt.Hour;
            if (message is SensorSummaryGenerationMessage)
            {
                SensorSummaryGenerationMessage sensorMessage = (SensorSummaryGenerationMessage) message;
                Sensor sensor = GetSensorByUuid(sensorMessage.Uid);                

                if (sensor is FlowSensor || sensor is EnergySensor)
                {
                    GenerateDailySummary(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    GenerateHourlySummary(sensor, summaryDate, summaryHour, sensorMessage.Value, sensorMessage.DataLoggedAt);                   
                    return;
                }
                //if (sensor is PressureSensor || sensor is LevelSensor || sensor is BatteryVoltageDetector)
                //{
                //    GenerateDailyAverageData(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                //    GenerateHourlyAverageData(sensor, summaryDate, summaryHour, sensorMessage.Value, sensorMessage.DataLoggedAt);
                //    return;
                //}
                //if (sensor is ChlorinePresenceDetector || sensor is ACPresenceDetector)
                //{
                //    GenerateSensorOnOffSummaryData(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                //}
            }
            //if (message is MotorSummaryGenerationMessage)
            //{
            //    MotorSummaryGenerationMessage motorMessage = (MotorSummaryGenerationMessage)message;
            //    Motor motor = GetMotorByUuid(motorMessage.Uid);
            //    GenerateMotorOnOffSummaryData(motor, motorMessage.MotorStatus, motorMessage.DataLoggedAt);
            //}
        }

        private void GenerateMotorOnOffSummaryData(Motor motor, string motorStatus, DateTime dataLoggedAt)
        {
            MotorOnOffSummaryData onOffData = GetMotorOnOffData(motor.MotorID);

            if (onOffData != null)
            {
                if (motorStatus == Motor.ON)
                {
                    onOffData.OnDateTime = dataLoggedAt;
                    onOffData.Duration = (dataLoggedAt - onOffData.OffDateTime).TotalSeconds;
                    onOffData.ProcessAt = dataLoggedAt;
                }
            }
            else if (motorStatus == Motor.OFF)
            {
                CreateMotorOnOffSummaryData(motorStatus, motor, dataLoggedAt);
            }
        }
        private void CreateMotorOnOffSummaryData(string motorStatus, Motor motor, DateTime dataLoggedAt)
        {
            MotorOnOffSummaryData data = new MotorOnOffSummaryData();

            data.OffDateTime = dataLoggedAt;
            data.Motor = motor;
            data.ProcessAt = dataLoggedAt;

            _wmsdbcontext.MotorOnOffSummaryData.Add(data);
        }
        private void GenerateSensorOnOffSummaryData(Sensor sensor, DateTime summaryDate, decimal dataValue, DateTime loggedAt)
        {
            SensorOnOffSummaryData onOffData = GetSensorOnOffData(sensor.SensorId, summaryDate);

            if (onOffData != null)
            {
                if (dataValue != 0)
                {
                    onOffData.OnDateTime = loggedAt;
                    onOffData.Duration = (loggedAt - onOffData.OffDateTime).TotalSeconds;
                    onOffData.ProcessAt = loggedAt;
                }
            }
            else if (dataValue == 0)
            {
                CreateSensorOnOffSummaryData(dataValue, sensor, loggedAt);               
            }          
        }
        private void CreateSensorOnOffSummaryData(decimal dataValue, Sensor sensor, DateTime loggedAt)
        {
            SensorOnOffSummaryData data = new SensorOnOffSummaryData();

            data.OffDateTime = loggedAt;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorOnOffSummaryData.Add(data);
        }
        private void GenerateHourlyAverageData(Sensor sensor, DateTime summaryDate, int summaryHour, decimal dataValue, DateTime loggedAt)
        {
            SensorHourlyAverageData hourlyAverage = GetHourlyAverageData(sensor.SensorId, summaryDate, summaryHour);

            if (hourlyAverage != null)
            {
                hourlyAverage.DataValue += dataValue;
                hourlyAverage.DataCount += 1;
                hourlyAverage.AverageValue = hourlyAverage.DataValue / hourlyAverage.DataCount;
                hourlyAverage.ProcessAt = loggedAt;
            }
            else
            {
                CreateHourlyAverageSensorData(summaryDate, summaryHour, dataValue, sensor, loggedAt);
            }
        }
        private void GenerateDailyAverageData(Sensor sensor, DateTime summaryDate, decimal dataValue, DateTime loggedAt)
        {
            SensorDailyAverageData dailyAverage = GetDailyAverageData(sensor.SensorId, summaryDate);

            if (dailyAverage != null)
            {
                dailyAverage.DataValue += dataValue;
                dailyAverage.DataCount += 1;
                dailyAverage.AverageValue = dailyAverage.DataValue / dailyAverage.DataCount;
                dailyAverage.ProcessAt = loggedAt;
            }
            else
            {
                CreateDailyAverageSensorData(summaryDate, dataValue, sensor, loggedAt);
            }
        }
        private void GenerateHourlySummary(Sensor sensor, DateTime summaryDate, int summaryHour, decimal dataValue, DateTime loggedAt)
        {
            SensorHourlySummaryData hourlySummary = GetHourlySummary(sensor.SensorId, summaryDate, summaryHour);

            if (hourlySummary != null)
            {
                hourlySummary.DataValue = hourlySummary.DataValue + (dataValue - hourlySummary.ReceivedValue);
                hourlySummary.ReceivedValue = dataValue;
                hourlySummary.ProcessAt = loggedAt;
            }
            else
            {
                CreateHourlySummarySensorData(summaryDate, summaryHour, dataValue, sensor, loggedAt);
            }
        }
        private void GenerateDailySummary(Sensor sensor, DateTime summaryDate, decimal dataValue, DateTime loggedAt)
        {
            SensorDailySummaryData dailySummary = GetDailySummary(sensor.SensorId, summaryDate);

            if (dailySummary != null)
            {
                dailySummary.DataValue = dailySummary.DataValue + (dataValue - dailySummary.ReceivedValue);
                dailySummary.ReceivedValue = dataValue;
                dailySummary.ProcessAt = loggedAt;

                sensor.CurrentValue = dailySummary.DataValue;
                if (sensor is FlowSensor)
                {
                    ((FlowSensor) sensor).CumulativeValue = dailySummary.ReceivedValue;
                }
                else
                {
                    ((EnergySensor)sensor).CumulativeValue = dailySummary.ReceivedValue;
                }
            }
            else
            {
                CreateDailySummarySensorData(summaryDate, dataValue, sensor, loggedAt);
            }
        }
        private Sensor GetSensorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Sensors where c.UUID == uuid select c).Single(); ;
        }
        private Motor GetMotorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Motors where c.UUID == uuid select c).Single(); ;
        }
        private void CreateHourlySummarySensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorHourlySummaryData data = new SensorHourlySummaryData();
            SensorHourlySummaryData lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate, hour-1);       

            if (lastDaySummary == null)
            {
                lastDaySummary = GetLastHourSummary(sensor.SensorId, summaryDate.AddDays(-1));
            }

            data.DataDate = summaryDate;
            data.DataHour = hour;         
            if (lastDaySummary != null)
            {
                data.DataValue = value - lastDaySummary.ReceivedValue;
            }
            else
            {
                data.DataValue = value;
            }
            data.ReceivedValue = value;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorHourlySummaryData.Add(data);
        }
        private void CreateDailySummarySensorData(DateTime summaryDate, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorDailySummaryData data = new SensorDailySummaryData();
            SensorDailySummaryData lastDaySummary = GetDailySummary(sensor.SensorId, summaryDate.AddDays(-1));

            data.DataDate = summaryDate;
            data.ReceivedValue = value;
            if (lastDaySummary != null)
            {
                data.DataValue = value - lastDaySummary.ReceivedValue;
            }
            else
            {
                data.DataValue = value;
            }
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            sensor.CurrentValue = data.DataValue;
            if (sensor is FlowSensor)
            {
                ((FlowSensor)sensor).CumulativeValue = data.ReceivedValue;
            }
            else
            {
                ((EnergySensor)sensor).CumulativeValue = data.ReceivedValue;
            }

            _wmsdbcontext.SensorDailySummaryData.Add(data);
        }
        private void CreateDailyAverageSensorData(DateTime summaryDate, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorDailyAverageData data = new SensorDailyAverageData();

            data.DataDate = summaryDate;
            data.DataValue = value;
            data.DataCount = 1;
            data.AverageValue = data.DataValue / data.DataCount;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorDailyAverageData.Add(data);
        }
        private void CreateHourlyAverageSensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorHourlyAverageData data = new SensorHourlyAverageData();

            data.DataDate = summaryDate;
            data.DataHour = hour;
            data.DataValue = value;
            data.DataCount = 1;
            data.AverageValue = data.DataValue / data.DataCount;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorHourlyAverageData.Add(data);
        }
        private SensorDailySummaryData GetDailySummary(int sensorId, DateTime summaryDate)
        {
            SensorDailySummaryData dailySummary = (from c in _wmsdbcontext.SensorDailySummaryData
                                                   where c.Sensor.SensorId == sensorId
                                                    && c.DataDate == summaryDate
                                                   select c).FirstOrDefault();
            return dailySummary;
        }
        private SensorHourlySummaryData GetHourlySummary(int sensorId, DateTime summaryDate, int summaryHour)
        {
            SensorHourlySummaryData hourlySummary = (from c in _wmsdbcontext.SensorHourlySummaryData
                                                     where c.Sensor.SensorId == sensorId
                                                           && c.DataDate == summaryDate
                                                           && c.DataHour == summaryHour
                                                     select c).FirstOrDefault();
            return hourlySummary;
        }
        private SensorHourlySummaryData GetLastHourSummary(int sensorId, DateTime summaryDate)
        {
            SensorHourlySummaryData hourlySummary = (from c in _wmsdbcontext.SensorHourlySummaryData
                                                     where c.Sensor.SensorId == sensorId
                                                           && c.DataDate == summaryDate
                                                     select c).OrderByDescending(o=>o.DataHour).FirstOrDefault();
            return hourlySummary;
        }
        private SensorDailyAverageData GetDailyAverageData(int sensorId, DateTime summaryDate)
        {
            SensorDailyAverageData dailyAverage = (from c in _wmsdbcontext.SensorDailyAverageData
                                                   where c.Sensor.SensorId == sensorId
                                                    && c.DataDate == summaryDate
                                                   select c).FirstOrDefault();
            return dailyAverage;
        }
        private SensorHourlyAverageData GetHourlyAverageData(int sensorId, DateTime summaryDate, int summaryHour)
        {
            SensorHourlyAverageData hourlyAverage = (from c in _wmsdbcontext.SensorHourlyAverageData
                                                    where c.Sensor.SensorId == sensorId
                                                    && c.DataDate == summaryDate
                                                    && c.DataHour == summaryHour
                                                    select c).FirstOrDefault();
            return hourlyAverage;
        }
        private SensorOnOffSummaryData GetSensorOnOffData(int sensorId, DateTime summaryDate)
        {
            SensorOnOffSummaryData onOffDatae = (from c in _wmsdbcontext.SensorOnOffSummaryData
                                                    where c.Sensor.SensorId == sensorId
                                                    && c.OnDateTime ==null
                                                     select c).OrderByDescending(o=>o.OffDateTime).FirstOrDefault();
            return onOffDatae;
        }
        private MotorOnOffSummaryData GetMotorOnOffData(int motorId)
        {
            MotorOnOffSummaryData onOffDatae = (from c in _wmsdbcontext.MotorOnOffSummaryData
                                                        where c.Motor.MotorID == motorId
                                                        && c.OnDateTime == null
                                                select c).OrderByDescending(o => o.OffDateTime).FirstOrDefault();
            return onOffDatae;
        }
    }
}
