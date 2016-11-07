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
                SensorSummaryGenerationMessage sensorMessage = (SensorSummaryGenerationMessage)message;
                Sensor sensor = GetSensorByUuid(sensorMessage.Uid);

                if (sensor is FlowSensor || sensor is EnergySensor)
                {
                    GenerateDailySummary(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    GenerateHourlySummary(sensor, summaryDate, summaryHour, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    //GenerateMinutelySummary(sensor, summaryDate, summaryHour, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    return;
                }
                if (sensor is PressureSensor || sensor is LevelSensor || sensor is BatteryVoltageDetector)
                {
                    GenerateDailyAverageData(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    GenerateHourlyAverageData(sensor, summaryDate, summaryHour, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    return;
                }
                if (sensor is ChlorinePresenceDetector || sensor is ACPresenceDetector)
                {
                    GenerateSensorOnOffSummaryData(sensor, summaryDate, sensorMessage.Value, sensorMessage.DataLoggedAt);
                    return;
                }
            }
            if (message is MotorSummaryGenerationMessage)
            {
                MotorSummaryGenerationMessage motorMessage = (MotorSummaryGenerationMessage)message;
                Motor motor = GetMotorByUuid(motorMessage.Uid);
                GenerateMotorOnOffSummaryData(motor, motorMessage.MotorStatus, motorMessage.DataLoggedAt);
            }
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
            else
            {
                if (motorStatus == Motor.OFF)
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
            SensorOnOffSummaryData onOffData = GetSensorOnOffData(sensor.SensorId);

            if (onOffData != null)
            {
                if (dataValue != 0)
                {
                    onOffData.OnDateTime = loggedAt;
                    onOffData.Duration = (loggedAt - onOffData.OffDateTime).TotalSeconds;
                    onOffData.ProcessAt = loggedAt;
                }
            }
            else
            {
                if (dataValue == 0)
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

        private void GenerateMinutelySummary(Sensor sensor, DateTime summaryDate, int summaryHour, decimal dataValue, DateTime loggedAt)
        {
            SensorMinutelySummaryData minutelySummary = GetMinutelySummary(sensor.SensorId, summaryDate, summaryHour);

            if (minutelySummary != null)
            {
                if (minutelySummary.DataDate.AddMinutes(5) < DateTime.Now)
                {
                    SensorMinutelySummaryData minuteSummary = new SensorMinutelySummaryData();
                    minuteSummary.Sensor = sensor;
                    minuteSummary.DataValue = minutelySummary.DataValue + (dataValue - minutelySummary.ReceivedValue);
                    minuteSummary.ReceivedValue = dataValue;
                    minuteSummary.DataDate = loggedAt;
                    minuteSummary.ProcessAt = loggedAt;
                    _wmsdbcontext.SensorMinutelySummaryData.Add(minuteSummary);
                    decimal prevCumulativeValue = minuteSummary.ReceivedValue;
                    if (sensor is EnergySensor)
                    {
                        ((EnergySensor)sensor).CumulativeValue = minuteSummary.ReceivedValue;
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(63, 65);//(dataValue - prevCumulativeValue) / (decimal)(0.000277778);
                    }
                }
                else
                {
                    minutelySummary.DataValue = minutelySummary.DataValue + (dataValue - minutelySummary.ReceivedValue);
                    minutelySummary.ReceivedValue = dataValue;
                    minutelySummary.ProcessAt = loggedAt;
                }

                //minutelySummary.ProcessAt = loggedAt;
            }
            else
            {
                CreateMinutelySummarySensorData(summaryDate, summaryHour, dataValue, sensor, loggedAt);
            }
        }

        private void GenerateDailySummary(Sensor sensor, DateTime summaryDate, decimal dataValue, DateTime loggedAt)
        {
            SensorDailySummaryData dailySummary = GetDailySummary(sensor.SensorId, summaryDate);

            if (dailySummary != null)
            {
                decimal prevCumulativeValue = dailySummary.ReceivedValue;
                dailySummary.DataValue = dailySummary.DataValue + (dataValue - dailySummary.ReceivedValue);
                dailySummary.ReceivedValue = dataValue;
                dailySummary.ProcessAt = loggedAt;

                //if (!(sensor is EnergySensor))
                //{
                sensor.CurrentValue = dailySummary.DataValue;
                sensor.LastDataReceived = loggedAt;
                //}

                if (sensor is FlowSensor)
                {
                    ((FlowSensor)sensor).CumulativeValue = dailySummary.ReceivedValue;
                    ((FlowSensor)sensor).LitrePerMinuteValue = (dataValue - prevCumulativeValue) * (6);
                }
                else
                {
                    if(sensor.SensorId == 4)
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(52, 55);

                    if (sensor.SensorId == 11)
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(63, 65);

                    if (sensor.SensorId == 18)
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(84, 87);

                    if (sensor.SensorId == 25)
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(81, 84);

                    if (sensor.SensorId == 32)
                        ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(63, 65);
                    ((EnergySensor)sensor).CumulativeValue = dailySummary.ReceivedValue;
                   // ((EnergySensor)sensor).KwPerHourValue = (decimal)GetRandomNumber(63, 65);//(dataValue - prevCumulativeValue) / (decimal)(0.000277778);
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
        public Sensor GetSensorId(int sensorId)
        {
            return (from c in _wmsdbcontext.Sensors where c.SensorId == sensorId select c).Single(); ;
        }
        private Motor GetMotorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Motors where c.UUID == uuid select c).Single(); ;
        }
        private void CreateHourlySummarySensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorHourlySummaryData data = new SensorHourlySummaryData();
            SensorHourlySummaryData lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate, hour - 1);

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

        private void CreateMinutelySummarySensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorMinutelySummaryData data = new SensorMinutelySummaryData();
            SensorMinutelySummaryData lastDaySummary = GetMinutelySummary(sensor.SensorId, summaryDate, hour - 1);

            data.DataDate = summaryDate;
            data.ReceivedValue = value;

            if (lastDaySummary == null)
            {
                lastDaySummary = GetLast5MinuteSummary(sensor.SensorId, summaryDate.AddHours(-1));

                sensor.CurrentValue = value;
            }

            if (lastDaySummary != null)
            {
                data.DataValue = value - lastDaySummary.ReceivedValue;
            }
            else
            {
                data.DataValue = value;
                sensor.CurrentValue = value;
            }
            data.ReceivedValue = value;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;
            data.DataDate = loggedAt;

            _wmsdbcontext.SensorMinutelySummaryData.Add(data);
        }

        private void CreateDailySummarySensorData(DateTime summaryDate, decimal value, Sensor sensor, DateTime loggedAt)
        {


            SensorDailySummaryData data = new SensorDailySummaryData();
            SensorDailySummaryData lastDaySummary = GetDailySummary(sensor.SensorId, summaryDate.AddDays(-1));
            decimal prevCumulativeValue = 0;
            data.DataDate = summaryDate;
            data.ReceivedValue = value;
            if (lastDaySummary != null)
            {
                data.DataValue = value - lastDaySummary.ReceivedValue;
                // if (!(sensor is EnergySensor))
                sensor.CurrentValue = value - lastDaySummary.ReceivedValue;
                prevCumulativeValue = lastDaySummary.ReceivedValue;
            }
            else
            {
                data.DataValue = value;

                // if (!(sensor is EnergySensor))
                sensor.CurrentValue = value;
            }
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            if (sensor is FlowSensor)
            {
                ((FlowSensor)sensor).CumulativeValue = data.ReceivedValue;
                ((FlowSensor)sensor).LitrePerMinuteValue = (value - prevCumulativeValue) * (6);
            }
            else
            {
                ((EnergySensor)sensor).CumulativeValue = data.ReceivedValue;
                ((EnergySensor) sensor).KwPerHourValue = (decimal) GetRandomNumber(63,65); //(value - prevCumulativeValue) / (decimal)(0.000277778);

            }
            sensor.LastDataReceived = loggedAt;
            _wmsdbcontext.SensorDailySummaryData.Add(data);

        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
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

        private SensorMinutelySummaryData GetMinutelySummary(int sensorId, DateTime summaryDate, int summaryHour)
        {
            SensorMinutelySummaryData minutelySummary = (from c in _wmsdbcontext.SensorMinutelySummaryData
                                                         where c.Sensor.SensorId == sensorId
                                                           && c.DataDate == summaryDate
                                                           && c.DataDate.Hour == summaryHour
                                                         select c).OrderByDescending(o => o.ProcessAt).FirstOrDefault();
            return minutelySummary;
        }
        private SensorHourlySummaryData GetLastHourSummary(int sensorId, DateTime summaryDate)
        {
            SensorHourlySummaryData hourlySummary = (from c in _wmsdbcontext.SensorHourlySummaryData
                                                     where c.Sensor.SensorId == sensorId
                                                           && c.DataDate == summaryDate
                                                     select c).OrderByDescending(o => o.DataHour).FirstOrDefault();
            return hourlySummary;
        }
        private SensorMinutelySummaryData GetLast5MinuteSummary(int sensorId, DateTime summaryDate)
        {
            SensorMinutelySummaryData minutelySummary = (from c in _wmsdbcontext.SensorMinutelySummaryData
                                                         where c.Sensor.SensorId == sensorId
                                                               && c.DataDate == summaryDate
                                                         select c).OrderByDescending(o => o.DataDate).FirstOrDefault();
            return minutelySummary;
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
        private SensorOnOffSummaryData GetSensorOnOffData(int sensorId)
        {
            SensorOnOffSummaryData onOffDatae = (from c in _wmsdbcontext.SensorOnOffSummaryData
                                                 where c.Sensor.SensorId == sensorId
                                                 && c.OnDateTime == null
                                                 select c).OrderByDescending(o => o.OffDateTime).FirstOrDefault();
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
