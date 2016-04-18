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

        public Dictionary<string, string> GetPumpStationOverView(int pumpStationId)
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
                {
                    var unitName = ((PressureSensor)sensor).Unit != null
                        ? ((PressureSensor)sensor).Unit.Name
                        : string.Empty;
                    dictonary.Add("PT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }


                else if (sensor is LevelSensor)
                {
                    var unitName = ((LevelSensor)sensor).Unit != null
                        ? ((LevelSensor)sensor).Unit.Name
                        : string.Empty;
                    dictonary.Add("LT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

                else if (sensor is EnergySensor)
                {
                    var unitName = ((EnergySensor)sensor).Unit != null
                        ? ((EnergySensor)sensor).Unit.Name
                        : string.Empty;
                    dictonary.Add("ET-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

                else if (sensor is FlowSensor)
                {
                    var unitName = ((FlowSensor)sensor).Unit != null
                        ? ((FlowSensor)sensor).Unit.Name
                        : string.Empty;
                    dictonary.Add("FT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

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

            if (model.ReportType == ReportType.Realtime)
            {
                return GeneratetSeriesDataRealTime(model);
            }

            return null;
        }
        private DrillDown GeneratetSeriesDataRealTime(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Live Data Review", null);

            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();

            model = SetupLiveData(model, pumpStation);
            return model;
        }
        private DrillDown GeneratetSeriesDataDaily(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Daily Data Review", "Data for " + model.ToDateTime.DayOfWeek);

            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();


            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                model = SetupDailyDataForFlowSensor(model, pumpStation);
            }

            if (model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
            {
                model = SetupDailyDataForEnergySensor(model, pumpStation);
            }

            return model;
        }

        private DrillDown SetupDailyDataForEnergySensor(DrillDown model, PumpStation pumpStation)
        {
            EnergySensor sensor = GetPumpStationSensor<EnergySensor>(pumpStation, model.TransmeType);

            model.Unit = "kw/h";
            ReportSeries data = new ReportSeries();
            data.name = "ET-" + sensor.UUID;
            data.data = GetDailyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown SetupDailyDataForFlowSensor(DrillDown model, PumpStation pumpStation)
        {
            var sensor = GetPumpStationSensor<FlowSensor>(pumpStation, model.TransmeType);
            model.Unit = "Meter";
            ReportSeries data = new ReportSeries();
            data.name = "FT-" + sensor.UUID;
            data.data = GetDailyData(ref model, sensor.SensorID);
            model.Series.Add(data);

            return model;
        }

        private DrillDown SetupLiveData(DrillDown model, PumpStation pumpStation)
        {
            foreach (var sensor in pumpStation.Sensors)
            {
                if (sensor is PressureSensor && model.TransmeType == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                {
                    ReportSeries data = new ReportSeries();
                    data.name = "PT" + "-" + sensor.UUID;
                    model.Unit = "Bar";
                    data.data = new List<double>() { (double)sensor.CurrentValue };
                    model.Series.Add(data);
                }

                if (sensor is EnergySensor && model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    ReportSeries data = new ReportSeries();
                    data.name = "ET" + "-" + sensor.UUID;
                    model.Unit = "kw/h";
                    data.data = new List<double>() { (double)sensor.CurrentValue };
                    model.Series.Add(data);
                }

                if (sensor is FlowSensor && model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
                {
                    ReportSeries data = new ReportSeries();
                    data.name = "FT" + "-" + sensor.UUID;
                    model.Unit = "litre/minute";
                    data.data = new List<double>() { (double)sensor.CurrentValue };
                    model.Series.Add(data);
                }

                if (sensor is ChlorinationSensor && model.TransmeType == Sensor.TransmitterType.CHLORINE_TRANSMITTER)
                {
                    ReportSeries data = new ReportSeries();
                    data.name = "CT" + "-" + sensor.UUID;
                    model.Unit = "";
                    data.data = new List<double>() { (double)sensor.CurrentValue };
                    model.Series.Add(data);
                }

                if (sensor is LevelSensor && model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                {
                    ReportSeries data = new ReportSeries();
                    data.name = "LT" + "-" + sensor.UUID;
                    model.Unit = "Meter";
                    data.data = new List<double>() { (double)sensor.CurrentValue };
                    model.Series.Add(data);
                }
            }
            

            return model;
        }

        private DrillDown GeneratetSeriesDataHourly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Hourly Data Review", "Data for Hour no = " + model.ToDateTime.Hour);
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();


            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                model = SetupHourlyDataForFlowSensor(model, pumpStation);
            }

            if (model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
            {
                model = SetupHourlyDataForEnergySensor(model, pumpStation);
            }


            return model;
        }

        private DrillDown SetupHourlyDataForEnergySensor(DrillDown model, PumpStation pumpStation)
        {
            EnergySensor sensor = GetPumpStationSensor<EnergySensor>(pumpStation, model.TransmeType);
            model.Unit = "kw/h";
            ReportSeries data = new ReportSeries();
            data.name = "ET-" + sensor.UUID;
            data.data = GetHourlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown SetupHourlyDataForFlowSensor(DrillDown model, PumpStation pumpStation)
        {
            FlowSensor sensor = GetPumpStationSensor<FlowSensor>(pumpStation, model.TransmeType);
            model.Unit = "Meter";
            ReportSeries data = new ReportSeries();
            data.name = "FT-" + sensor.UUID;
            data.data = GetHourlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown GeneratetSeriesDataWeekly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Weekly Data Review", "Data for Week no = " + model.Week);
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();


            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                model = SetupWeeklyDataForFlowSensor(model, pumpStation);
            }

            if (model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
            {
                model = SetupWeeklyDataForEnergySensor(model, pumpStation);
            }

            return model;
        }

        private DrillDown SetupWeeklyDataForEnergySensor(DrillDown model, PumpStation pumpStation)
        {
            EnergySensor sensor = GetPumpStationSensor<EnergySensor>(pumpStation, model.TransmeType);
            model.Unit = "kw/h";
            ReportSeries data = new ReportSeries();
            data.name = "ET-" + sensor.UUID;
            data.data = GetWeeklyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown SetupWeeklyDataForFlowSensor(DrillDown model, PumpStation pumpStation)
        {
            FlowSensor sensor = GetPumpStationSensor<FlowSensor>(pumpStation, model.TransmeType);
            model.Unit = "Metre";
            ReportSeries data = new ReportSeries();
            data.name = "FT-" + sensor.UUID;
            data.data = GetWeeklyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown GeneratetSeriesDataMonthly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Monthly Data Review", "Data for " + model.ToDateTime.ToString("MMM"));
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaID == model.SelectedPumpStationId).First();

            if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
            {
                model = SetupMonthlyDataForFlowSensor(model, pumpStation);
            }

            if (model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
            {
                model = SetupMonthlyDataForEnergySensor(model, pumpStation);
            }

            return model;
        }

        private static void SetGraphTitleAndSubTitle(ref DrillDown model, string title, string subtitle)
        {
            model.GraphTitle = title;
            model.GraphSubTitle = subtitle;

        }

        private DrillDown SetupMonthlyDataForEnergySensor(DrillDown model, PumpStation pumpStation)
        {
            EnergySensor sensor = GetPumpStationSensor<EnergySensor>(pumpStation, model.TransmeType);
            model.Unit = "kw/h";
            ReportSeries data = new ReportSeries();
            data.name = "ET-" + sensor.UUID;
            data.data = GetMonthlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown SetupMonthlyDataForFlowSensor(DrillDown model, PumpStation pumpStation)
        {
            model.XaxisCategory = new string[30];
            FlowSensor sensor = GetPumpStationSensor<FlowSensor>(pumpStation, model.TransmeType);
            model.Unit = "Metre";
            ReportSeries data = new ReportSeries();
            data.name = "FT-" + sensor.UUID;
            data.data = GetMonthlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private T GetPumpStationSensor<T>(PumpStation pumpStation, Sensor.TransmitterType type) where T : Sensor
        {
            foreach (var sensor in pumpStation.Sensors)
            {
                if (sensor is WMS.QueryModel.Sensors.PressureSensor && type == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                {
                    PressureSensor p = new PressureSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is LevelSensor && type == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                {
                    LevelSensor p = new LevelSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is EnergySensor && type == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    EnergySensor p = new EnergySensor() { SensorID = sensor.SensorID, UUID = sensor.UUID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }

                if (sensor is WMS.QueryModel.Sensors.FlowSensor && type == Sensor.TransmitterType.FLOW_TRANSMITTER)
                {
                    FlowSensor p = new FlowSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID };
                    return (T)Convert.ChangeType(p, typeof(T));
                }
            }
            return (T)Activator.CreateInstance(typeof(T));

        }

        private List<double> GetDailyData(ref DrillDown model, int sensorId)
        {
            model.XaxisCategory = new string[25];
            List<double> avgValue = new List<double>();
            for (int i = 0; i <= 24; i++)
            {
                if (model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER || model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddHours(i),
                    model.ToDateTime.AddHours(i + 1)));
                    model.XaxisCategory[i] = (i + 1).ToString();
                }
            }
            return avgValue;
        }
        private List<double> GetHourlyData(ref DrillDown model, int sensorId)
        {
            model.XaxisCategory = new string[13];
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
            model.XaxisCategory = new string[7];
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
            model.XaxisCategory = new string[30];
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
                return ((double)sensorDataList.Sum(x => x.Value));
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
