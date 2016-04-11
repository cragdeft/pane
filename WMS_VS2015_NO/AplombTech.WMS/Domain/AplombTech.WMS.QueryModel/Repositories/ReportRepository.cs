using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Reports;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;

namespace AplombTech.WMS.QueryModel.Repositories
{
    [DisplayName("Reports")]
    public class ReportRepository : AbstractFactoryAndRepository
    {
        public static void Menu(IMenu menu)
        {
            menu.AddAction("GoogleMap");
            menu.AddAction("ScadaMap");
            menu.AddAction("DrillDown");
            //menu.CreateSubMenu("Zone")
            //    .AddAction("CreateZone")
            //    .AddAction("FindZone")
            //    .AddAction("AllZones");
            //menu.CreateSubMenu("DMA")
            //    .AddAction("FindDMA");
            //menu.CreateSubMenu("PumpStation")
            //    .AddAction("FindPumpStation");
            //    .AddAction("AllCities");
            //menu.CreateSubMenu("উপজেলা")
            //    .AddAction("AddSubDistrict")
            //    .AddAction("BySubDistrictName")
            //    .AddAction("AllSubDistricts");
            //menu.CreateSubMenu("জেলা")
            //    .AddAction("AddDistrict")
            //    .AddAction("ByDistrictName")
            //    .AddAction("AllDistrict");
            //menu.CreateSubMenu("বিভাগ")
            //    .AddAction("AllDivisions");
            //menu.AddAction("CustomerDashboard");
        }

        [DisplayName("Google Map")]
        public ZoneGoogleMap GoogleMap()
        {
            var zones = Container.NewViewModel<ZoneGoogleMap>();
            zones.Zones = Container.Instances<Zone>().ToList();

            return zones;
        }
        [DisplayName("Drill Down")]
        public DrillDown DrillDown()
        {
            var model = Container.NewViewModel<DrillDown>();
            model.PumpStations = Container.Instances<PumpStation>().ToList();

            return model;
        }

        public ScadaMap ScadaMap()
        {
            var model = Container.NewViewModel<ScadaMap>();
            model.Zones = Container.Instances<Zone>().ToList();
            return model;
        }

        public List<DMA> GetDmaList(int zoneId)
        {
            var model = Container.Instances<DMA>().Where(x => x.Parent.AreaID == zoneId).ToList();
            return model;
        }

        public List<PumpStation> GetPumpStationList(int dmaId)
        {
            var model = Container.Instances<PumpStation>().Where(x => x.Parent.AreaID == dmaId).ToList();
            return model;
        }

        public List<Sensor> GetSensorData(int pumpStationId)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaID == pumpStationId).FirstOrDefault();
            //var temp = pumpStation.Sensors.ToList();
            return pumpStation.Sensors.ToList();
        }

        public Sensor GetPumpSingleSensor(int sensorId)
        {
            var model = Container.Instances<Sensor>().Where(x => x.SensorID == sensorId).SingleOrDefault();
            return model;
        }

        public Dictionary<string,string> GetPumpStationOverView(int pumpStationId)
        {
            var model = Container.Instances<PumpStation>().Where(x => x.AreaID == pumpStationId).FirstOrDefault();
            Dictionary<string, string> dictonary = new Dictionary<string, string>();
            GetSensorDataDictonary(model, dictonary);

            return dictonary;

        }

        private Dictionary<string, string> GetSensorDataDictonary(PumpStation model, Dictionary<string, string> dictonary)
        {
            foreach (var sensor in model.Sensors)
            {
                if (sensor is PressureSensor)
                    dictonary.Add("PT-" + sensor.UUID, sensor.CurrentValue + " " + ((PressureSensor) sensor).Unit.Name);

                else if (sensor is LevelSensor)
                    dictonary.Add("LT-" + sensor.UUID, sensor.CurrentValue + " " + ((LevelSensor) sensor).Unit.Name);

                else if (sensor is EnergySensor)
                    dictonary.Add("ET-" + sensor.UUID, sensor.CurrentValue + " " + ((EnergySensor) sensor).Unit.Name);

                else if (sensor is FlowSensor)
                    dictonary.Add("FT-" + sensor.UUID, sensor.CurrentValue + " " + ((FlowSensor) sensor).Unit.Name);

                else if (sensor is ChlorinationSensor)
                {
                    string cholorinationValue = null;
                    cholorinationValue = sensor.CurrentValue > 0 ? "Cholorination on" : "Cholorination off";
                    dictonary.Add("CT-" + sensor.UUID, cholorinationValue);
                }
            }

            return dictonary;
        }

        public DrillDown GetReportData(DrillDown model)
        {
            if (model.ReportType == ReportType.Daily)
            {
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, model.Day);
                return GeneratetSeriesDataDaily(model);
            }
            if (model.ReportType == ReportType.Hourly)
            {
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, model.Day, model.Hour - 1, 0, 0);
                return GeneratetSeriesDataHourly(model);
            }
            if (model.ReportType == ReportType.Weekly)
            {
                model.ToDateTime = new DateTime(model.Year, 1, 1).AddDays((model.Week - 1) * 7);
                return GeneratetSeriesDataWeekly(model);
            }

            if (model.ReportType == ReportType.Monthly)
            {
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, 1);
                return GeneratetSeriesDataMonthly(model);
            }

            return null;
        }

        private DrillDown GeneratetSeriesDataDaily(DrillDown model)
        {
            model.GraphTitle = "Daily Data Review";
            model.GraphSubTitle = "Data for " + model.ToDateTime.DayOfWeek;
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();
            model.XaxisCategory = new string[25];

            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                PressureSensor sensor = GetPumpStationSensor<PressureSensor>(pumpStation,model.TransmeType);
                model.Unit = "Bar";
                ReportSeries data = new ReportSeries();
                data.name = "PT-" + sensor.UUID;
                data.data = GetDailyData(ref model, sensor.SensorID);
                model.Series.Add(data);
            }

            return model;
        }

        private DrillDown GeneratetSeriesDataHourly(DrillDown model)
        {
            model.GraphTitle = "Hourly Data Review";
            model.GraphSubTitle = "Data for Hour no=" + model.ToDateTime.Hour;
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();
            model.XaxisCategory = new string[13];

            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                PressureSensor sensor = GetPumpStationSensor<PressureSensor>(pumpStation, model.TransmeType);
                model.Unit = "Bar";
                ReportSeries data = new ReportSeries();
                data.name = "PT-" + sensor.UUID;
                data.data = GetHourlyData(ref model, sensor.SensorID);
                model.Series.Add(data);
            }

            return model;
        }

        private DrillDown GeneratetSeriesDataWeekly(DrillDown model)
        {
            model.GraphTitle = "Weekly Data Review";
            model.GraphSubTitle = "Data for Week no=" + model.Week;
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();
            model.XaxisCategory = new string[7];

            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                PressureSensor sensor = GetPumpStationSensor<PressureSensor>(pumpStation, model.TransmeType);
                model.Unit = "Bar";
                ReportSeries data = new ReportSeries();
                data.name = "PT-" + sensor.UUID;
                data.data = GetWeeklyData(ref model, sensor.SensorID);
                model.Series.Add(data);
            }

            return model;
        }

        private DrillDown GeneratetSeriesDataMonthly(DrillDown model)
        {
            model.GraphTitle = "Monthly Data Review";
            model.GraphSubTitle = "Data for " + model.ToDateTime.ToString("MMM");
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();
            model.XaxisCategory = new string[30];

            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                PressureSensor sensor = GetPumpStationSensor<PressureSensor>(pumpStation, model.TransmeType);
                model.Unit = "Bar";
                ReportSeries data = new ReportSeries();
                data.name = "PT-" + sensor.UUID;
                data.data = GetMonthlyData(ref model, sensor.SensorID);
                model.Series.Add(data);
            }

            return model;
        }

        private T GetPumpStationSensor<T>(PumpStation pumpStation, Sensor.TransmitterType type)
        {
            foreach (var sensor in pumpStation.Sensors )
            {
                if (sensor is WMS.QueryModel.Sensors.PressureSensor  && type == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                {
                    PressureSensor p = new PressureSensor() {SensorID = sensor.SensorID};
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is LevelSensor && type == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                {
                    LevelSensor p = new LevelSensor() { SensorID = sensor.SensorID};
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is EnergySensor && type == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    EnergySensor p = new EnergySensor() { SensorID = sensor.SensorID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is FlowSensor && type == Sensor.TransmitterType.FLOW_TRANSMITTER)
                {
                    FlowSensor p = new FlowSensor() { SensorID = sensor.SensorID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }

        private List<double> GetDailyData(ref DrillDown model,int sensorId)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i <= 24; i++)
            {
                if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddHours(i),
                    model.ToDateTime.AddHours(i + 1)));
                    model.XaxisCategory[i] = (i + 1).ToString();
                }

                //if (model.TransmeType == Sensor.TransmitterType.PRESSURE_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                //{
                //    avgValue.Add(GetCurrentDataWithinTime(sensorId, model.ToDateTime.AddHours(i),
                //    model.ToDateTime.AddHours(i + 1)));
                //    model.XaxisCategory[i] = (i + 1).ToString();
                //}
            }
            return avgValue;
        }

        private List<double> GetHourlyData(ref DrillDown model, int sensorId)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i <= 12; i++)
            {
                if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddMinutes(i == 0 ? 0 : i + 5),
                    model.ToDateTime.AddMinutes(i == 0 ? 5 : i * 5)));
                    model.XaxisCategory[i] = model.ToDateTime.AddMinutes(i * 5).ToShortTimeString();
                }
            }
            return avgValue;
        }

        private List<double> GetWeeklyData(ref DrillDown model, int sensorId)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddDays(i),
                    model.ToDateTime.AddDays(i + 1)));
                    model.XaxisCategory[i] = (i + 1).ToString();
                }
            }
            return avgValue;
        }

        private List<double> GetMonthlyData(ref DrillDown model, int sensorId)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i < 30; i++)
            {
                if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddDays(i),
                    model.ToDateTime.AddDays(i + 1)));
                    model.XaxisCategory[i] = model.ToDateTime.AddDays(i).Day.ToString();
                }
            }
            return avgValue;
        }

        private double GetTotalDataWithinTime(int sensorId, DateTime from, DateTime to)
        {
            List<SensorData> sensorDataList = Container.Instances<SensorData>()
                   .Where(x => (x.Sensor.SensorID == sensorId && x.LoggedAt >= from && x.LoggedAt <= to)).ToList();

            if (sensorDataList != null)
                return ((double) sensorDataList.Sum(x => x.Value));
            else
                return 0;
        }

        private double GetCurrentDataWithinTime(int sensorId, DateTime from, DateTime to)
        {
            SensorData sensorData = Container.Instances<SensorData>()
                   .Where(x => (x.Sensor.SensorID == sensorId && x.LoggedAt >= from && x.LoggedAt <= to)).FirstOrDefault();

            if (sensorData != null)
                return ((double)sensorData.Value);
            else
                return 0;
        }
    }
}
