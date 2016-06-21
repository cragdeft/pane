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
            Sensor sensor = GetSensorByUuid(message.SensorUUID);
            DateTime summaryDate = new DateTime(message.DataLoggedAt.Year,message.DataLoggedAt.Month,message.DataLoggedAt.Day);
            int summaryHour = message.DataLoggedAt.Hour;

            GenerateHourlySummar(sensor, summaryDate, summaryHour, message.Value, message.DataLoggedAt);
            GenerateDailySummar(sensor, summaryDate, message.Value, message.DataLoggedAt);
        }

        private void GenerateHourlySummar(Sensor sensor, DateTime summaryDate, int summaryHour, decimal dataValue, DateTime loggedAt)
        {
            SensorSummaryDataHourly hourlySummary = GetHourlySummary(sensor.SensorId, summaryDate, summaryHour);

            if (hourlySummary != null)
            {
                if (loggedAt > hourlySummary.ProcessAt)
                {
                    SensorSummaryDataHourly lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate, summaryHour - 1);
                    if (lastDaySummary != null)
                    {
                        lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate.AddDays(-1), 23);
                    }
                    hourlySummary.ReceivedValue = dataValue;
                    if (lastDaySummary != null)
                    {
                        hourlySummary.DataValue = dataValue - lastDaySummary.ReceivedValue;
                    }
                    else
                    {
                        hourlySummary.DataValue = dataValue;
                    }
                    hourlySummary.DataCount += 1;
                    hourlySummary.ProcessAt = loggedAt;
                }
            }
            else
            {
                CreateHourlySummarySensorData(summaryDate, summaryHour, dataValue, sensor, loggedAt);
            }
        }
        private void GenerateDailySummar(Sensor sensor, DateTime summaryDate, decimal dataValue, DateTime loggedAt)
        {
            SensorSummaryDataDaily dailySummary = GetDailySummary(sensor.SensorId, summaryDate);

            if (dailySummary != null)
            {
                SensorSummaryDataDaily lastDaySummary = GetDailySummary(sensor.SensorId, summaryDate.AddDays(-1));
                if (loggedAt > dailySummary.ProcessAt)
                {
                    if (lastDaySummary != null)
                    {
                        dailySummary.ReceivedValue = dataValue;
                        dailySummary.DataValue = dataValue - lastDaySummary.DataValue;
                    }
                    else
                    {
                        dailySummary.ReceivedValue = dataValue;
                        dailySummary.DataValue = dataValue;
                    }
                    dailySummary.DataCount += 1;
                    dailySummary.ProcessAt = loggedAt;
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
        private void CreateHourlySummarySensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorSummaryDataHourly data = new SensorSummaryDataHourly();
            SensorSummaryDataHourly lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate, hour-1);       

            if (lastDaySummary == null)
            {
                lastDaySummary = GetHourlySummary(sensor.SensorId, summaryDate.AddDays(-1),23);
            }

            data.DataDate = summaryDate;
            data.DataHour = hour;
            data.ReceivedValue = value;
            if (lastDaySummary != null)
            {
                data.DataValue = value - lastDaySummary.ReceivedValue;
            }
            else
            {
                data.DataValue = value;
            }
            data.DataCount += 1;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorSummaryDataHourly.Add(data);
        }
        private void CreateDailySummarySensorData(DateTime summaryDate, decimal value, Sensor sensor, DateTime loggedAt)
        {
            SensorSummaryDataDaily data = new SensorSummaryDataDaily();
            SensorSummaryDataDaily lastDaySummary = GetDailySummary(sensor.SensorId, summaryDate.AddDays(-1));

            data.DataDate = summaryDate;
            if (lastDaySummary != null)
            {
                data.ReceivedValue = value;
                data.DataValue = value - lastDaySummary.ReceivedValue;
            }
            else
            {
                data.ReceivedValue = value;
                data.DataValue = value;
            }
            data.DataCount = 1;
            data.Sensor = sensor;
            data.ProcessAt = loggedAt;

            _wmsdbcontext.SensorSummaryDataDaily.Add(data);
        }
        private SensorSummaryDataDaily GetDailySummary(int sensorId, DateTime summaryDate)
        {
            SensorSummaryDataDaily dailySummary = (from c in _wmsdbcontext.SensorSummaryDataDaily
                                                   where c.Sensor.SensorId == sensorId
                                                    && c.DataDate == summaryDate
                                                   select c).FirstOrDefault();
            return dailySummary;
        }
        private SensorSummaryDataHourly GetHourlySummary(int sensorId, DateTime summaryDate, int summaryHour)
        {
            SensorSummaryDataHourly hourlySummary = (from c in _wmsdbcontext.SensorSummaryDataHourly
                                                     where c.Sensor.SensorId == sensorId
                                                           && c.DataDate == summaryDate
                                                           && c.DataHour == summaryHour
                                                     select c).FirstOrDefault();
            return hourlySummary;
        }
    }
}
