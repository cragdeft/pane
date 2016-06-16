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

            GenerateHourlySummar(sensor, summaryDate, summaryHour, message.Value);
            GenerateDailySummar(sensor, summaryDate, message.Value);
        }

        private void GenerateHourlySummar(Sensor sensor, DateTime summaryDate, int summaryHour, decimal dataValue)
        {
            SensorSummaryDataHourly hourlySummary = (from c in _wmsdbcontext.SensorSummaryDataHourly
                                                     where c.Sensor.SensorId == sensor.SensorId
                                                     && c.DataDate == summaryDate
                                                     && c.DataHour == summaryHour
                                                     select c).Single();

            if (hourlySummary != null)
            {
                hourlySummary.DataValue += dataValue;
            }
            else
            {
                CreateHourlySummarySensorData(summaryDate, summaryHour, dataValue, sensor);
            }
        }
        private void GenerateDailySummar(Sensor sensor, DateTime summaryDate, decimal dataValue)
        {
            SensorSummaryDataDaily dailySummary = (from c in _wmsdbcontext.SensorSummaryDataDaily
                                                    where c.Sensor.SensorId == sensor.SensorId
                                                     && c.DataDate == summaryDate
                                                     select c).Single();
            if (dailySummary != null)
            {
                dailySummary.DataValue += dataValue;
            }
            else
            {
                CreateDailySummarySensorData(summaryDate, dataValue, sensor);
            }
        }
        private Sensor GetSensorByUuid(string uuid)
        {
            return (from c in _wmsdbcontext.Sensors where c.UUID == uuid select c).Single(); ;
        }
        private void CreateHourlySummarySensorData(DateTime summaryDate, int hour, decimal value, Sensor sensor)
        {
            SensorSummaryDataHourly data = new SensorSummaryDataHourly();

            data.DataDate = summaryDate;
            data.DataHour = hour;
            data.DataValue = value;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            _wmsdbcontext.SensorSummaryDataHourly.Add(data);
        }
        private void CreateDailySummarySensorData(DateTime summaryDate, decimal value, Sensor sensor)
        {
            SensorSummaryDataDaily data = new SensorSummaryDataDaily();

            data.DataDate = summaryDate;
            data.DataValue = value;
            data.Sensor = sensor;
            data.ProcessAt = DateTime.Now;

            _wmsdbcontext.SensorSummaryDataDaily.Add(data);
        }
    }
}
