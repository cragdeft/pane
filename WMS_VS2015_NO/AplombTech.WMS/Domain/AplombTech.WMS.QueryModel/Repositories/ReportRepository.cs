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
using AplombTech.WMS.QueryModel.UserAccounts;
using AplombTech.WMS.QueryModel.Features;
using AplombTech.WMS.QueryModel.Motors;
using NakedObjects.Core.Util.Enumer;

namespace AplombTech.WMS.QueryModel.Repositories
{
    [DisplayName("Reports")]
    public class ReportRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public LoggedInUserInfoRepository LoggedInUserInfoRepository { set; protected get; }
        #endregion

        public static void Menu(IMenu menu)
        {
            menu.AddAction("GoogleMap");
            menu.AddAction("ScadaMap");
            menu.AddAction("DrillDown");
            menu.AddAction("Summary");
            menu.AddAction("UnderThresold");
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
        public ZoneGoogleMap GetSingleAreaGoogleMap(int zoneId)
        {
            var zones = Container.NewViewModel<ZoneGoogleMap>();
            zones.Zones = Container.Instances<Zone>().Where(x => x.AreaId == zoneId).ToList();

            return zones;
        }
        public bool HideUnderGoogleMap()
        {
            IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.GoogleMap
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        [DisplayName("Drill Down")]
        public DrillDown DrillDown()
        {
            var model = Container.NewViewModel<DrillDown>();
            model.PumpStations = Container.Instances<PumpStation>().ToList();

            return model;
        }
        public bool HideUnderDrillDown()
        {
            IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.DrillDown
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        [DisplayName("Under Thresold")]
        public UnderThresold UnderThresold()
        {
            var model = Container.NewViewModel<UnderThresold>();
            model.PumpStations = Container.Instances<PumpStation>().ToList();

            return model;
        }
        public bool HideUnderThresold()
        {
            IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.UnderThreshold
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        public Summary Summary()
        {
            var model = Container.NewViewModel<Summary>();
            model.Zones = Container.Instances<Zone>().ToList();

            return model;
        }
        public bool HideSummary()
        {
            IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.SummaryReport
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        public ScadaMap ScadaMap()
        {
            var model = Container.NewViewModel<ScadaMap>();
            model.Zones = Container.Instances<Zone>().ToList();
            return model;
        }
        public bool HideScadaMap()
        {
            IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.ScadaMap
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }

        public List<DMA> GetDmaList(int zoneId)
        {
            var model = Container.Instances<DMA>().Where(x => x.Parent.AreaId == zoneId).ToList();
            return model;
        }

        public List<PumpStation> GetPumpStationList(int dmaId)
        {
            var model = Container.Instances<PumpStation>().Where(x => x.Parent.AreaId == dmaId).ToList();
            return model;
        }

        public List<Sensor> GetSensorData(int pumpStationId)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();
            //var temp = pumpStation.Sensors.ToList();
            return pumpStation.Sensors.ToList();
        }

        public MotorData GetPumpMotorData(int pumpStationId)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();

            PumpMotor motor = pumpStation.PumpMotors;
            if (motor == null) return null;
            var motorData = Container.Instances<MotorData>().Where(x => x.Motor.MotorID == motor.MotorID).OrderByDescending(x => x.ProcessAt).FirstOrDefault();

            return motorData;
        }

        public MotorData GetCholorineMotorData(int pumpStationId)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();

            ChlorineMotor motor = pumpStation.ChlorineMotors;
            if (motor == null) return null;
            var motorData = Container.Instances<MotorData>().Where(x => x.Motor.MotorID == motor.MotorID).OrderByDescending(x => x.ProcessAt).FirstOrDefault();

            return motorData;
        }

        public Sensor GetPumpSingleSensor(int sensorId)
        {
            var model = Container.Instances<Sensor>().Where(x => x.SensorID == sensorId).SingleOrDefault();
            return model;
        }

        public Sensor GetPumpSingleSensorByUid(string uid)
        {
            var model = Container.Instances<Sensor>().Where(x => x.UUID == uid).SingleOrDefault();
            return model;
        }

        public PumpStation GetPumpStationById(int pumpStationId)
        {
            return Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).SingleOrDefault();
        }

        public Dictionary<string, string> GetPumpStationOverView(int pumpStationId)
        {
            var model = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();
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
                    var unitName = sensor.UnitName;
                    dictonary.Add("PT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }


                else if (sensor is LevelSensor)
                {
                    var unitName = sensor.UnitName;
                    dictonary.Add("LT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

                else if (sensor is EnergySensor)
                {
                    var unitName = sensor.UnitName;
                    dictonary.Add("ET-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

                else if (sensor is FlowSensor)
                {
                    var unitName = sensor.UnitName;
                    dictonary.Add("FT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }

                else if (sensor is ChlorinePresenceDetector)
                {
                    string cholorinationValue = null;
                    cholorinationValue = sensor.CurrentValue == 0 ? "Cholorination on" : "Cholorination off";
                    dictonary.Add("CPD-" + sensor.UUID, cholorinationValue);
                }
                else if (sensor is ACPresenceDetector)
                {
                    string acpValue = null;
                    acpValue = sensor.CurrentValue == 0 ? "ACP on" : "ACP off";
                    dictonary.Add("ACP-" + sensor.UUID, acpValue);
                }
                else if (sensor is BatteryVoltageDetector)
                {
                    var unitName = sensor.UnitName;
                    dictonary.Add("BV-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
                }
            }

            return dictonary;
        }

        #region DrillDown Report Data
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
                model.ToDateTime = DateTime.Now;
                return GeneratetSeriesDataRealTime(model);
            }

            return null;
        }

        public UnderThresold GetUnderThresoldtData(UnderThresold model)
        {
            if (model.ReportType == ReportType.Daily)
            {
                model.FromDateTime = new DateTime(model.Year, (int)model.Month, model.Day, 0, 0, 0);
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, model.Day, 23, 59, 59);

            }
            if (model.ReportType == ReportType.Hourly)
            {
                model.FromDateTime = new DateTime(model.Year, (int)model.Month, model.Day, model.Hour - 1, 0, 0);
                model.ToDateTime = new DateTime(model.Year, (int)model.Month, model.Day, model.Hour - 1, 59, 59);

            }
            if (model.ReportType == ReportType.Weekly)
            {
                model.FromDateTime = new DateTime(model.Year, 1, 1).AddDays((model.Week - 1) * 7);
                model.ToDateTime = model.FromDateTime.AddDays(7);
            }

            if (model.ReportType == ReportType.Monthly)
            {
                model.FromDateTime = new DateTime(model.Year, (int)model.Month, 1);
                model.ToDateTime = model.FromDateTime.AddDays(DateTime.DaysInMonth(model.Year, (int)model.Month));

            }
            return GenerateUnderThresoldData(model);
        }

        private UnderThresold GenerateUnderThresoldData(UnderThresold model)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
            Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            model.SensorDatas = GetCurrentDataListWithinTime(sensor, model.FromDateTime, model.ToDateTime);
            return model;
        }

        private DrillDown GeneratetSeriesDataRealTime(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Live Data Review", null);
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();

            return SetupLiveData(model, pumpStation);
        }
        private DrillDown GeneratetSeriesDataDaily(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Daily Data Review", "Data for " + model.ToDateTime.DayOfWeek);
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
            model.SelectedSensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            return SetupDailyData(model, pumpStation);
        }
        private DrillDown SetupDailyData(DrillDown model, PumpStation pumpStation)
        {
            Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            ReportSeries data = new ReportSeries();
            data.name = model.TransmeType.ToString().Replace("_", " ") + "-" + sensor.UUID;
            data.data = GetDailyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }

        private DrillDown SetupLiveData(DrillDown model, PumpStation pumpStation)
        {
            var sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            ReportSeries data = new ReportSeries();
            data.name = model.TransmeType.ToString().Replace("_", " ") + "-" + sensor.UUID;
            if (sensor is EnergySensor)
            {
                data.data = new List<double>() { (double)((EnergySensor)sensor).CumulativeValue };
            }
            else if (sensor is FlowSensor)
            {
                data.data = new List<double>() { (double)((FlowSensor)sensor).CumulativeValue };
            }
            else
                data.data = new List<double>() { (double)sensor.CurrentValue };
            model.Series.Add(data);
            return model;
        }
        private DrillDown GeneratetSeriesDataHourly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Hourly Data Review", "Data between Hour no = " + model.ToDateTime.Hour + " to " + ((model.ToDateTime.Hour) + 1));
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
            model.SelectedSensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            return SetupHourlyData(model, pumpStation);
        }
        private DrillDown SetupHourlyData(DrillDown model, PumpStation pumpStation)
        {
            Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            ReportSeries data = new ReportSeries(sensor.MinimumValue);
            data.name = model.TransmeType.ToString().Replace("_", " ") + "-" + sensor.UUID;
            data.data = GetHourlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }
        private DrillDown GeneratetSeriesDataWeekly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Weekly Data Review", "Data for Week no = " + model.Week);
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
            model.SelectedSensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            return SetupWeeklyData(model, pumpStation);
        }
        private DrillDown SetupWeeklyData(DrillDown model, PumpStation pumpStation)
        {
            Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            ReportSeries data = new ReportSeries();
            data.name = model.TransmeType.ToString().Replace("_", " ") + "-" + sensor.UUID;
            data.data = GetWeeklyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }
        private DrillDown GeneratetSeriesDataMonthly(DrillDown model)
        {
            SetGraphTitleAndSubTitle(ref model, "Monthly Data Review", "Data for " + model.ToDateTime.ToString("MMM"));
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
            model.SelectedSensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            return SetupMonthlyData(model, pumpStation);
        }
        private static void SetGraphTitleAndSubTitle(ref DrillDown model, string title, string subtitle)
        {
            model.GraphTitle = title;
            model.GraphSubTitle = subtitle;

        }
        private DrillDown SetupMonthlyData(DrillDown model, PumpStation pumpStation)
        {
            Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
            ReportSeries data = new ReportSeries();
            data.name = data.name = model.TransmeType.ToString().Replace("_", " ") + "-" + sensor.UUID;
            data.data = GetMonthlyData(ref model, sensor.SensorID);
            model.Series.Add(data);
            return model;
        }
        private T GetPumpStationSensor<T>(PumpStation pumpStation, ref DrillDown model) where T : Sensor
        {
            foreach (var sensor in pumpStation.Sensors)
            {
                if (sensor is WMS.QueryModel.Sensors.PressureSensor && model.TransmeType == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                {
                    Sensor p = new PressureSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is LevelSensor && model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                {
                    Sensor p = new LevelSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is EnergySensor && model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    Sensor p = new EnergySensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue, CumulativeValue = ((EnergySensor)sensor).CumulativeValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is WMS.QueryModel.Sensors.FlowSensor && model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
                {
                    Sensor p = new FlowSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue,CumulativeValue = ((FlowSensor)sensor).CumulativeValue};
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }
                if (sensor is WMS.QueryModel.Sensors.ChlorinePresenceDetector && model.TransmeType == Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR)
                {
                    Sensor p = new ChlorinePresenceDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }

                if (sensor is WMS.QueryModel.Sensors.ACPresenceDetector && model.TransmeType == Sensor.TransmitterType.AC_PRESENCE_DETECTOR)
                {
                    Sensor p = new ACPresenceDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }

                if (sensor is WMS.QueryModel.Sensors.BatteryVoltageDetector && model.TransmeType == Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR)
                {
                    Sensor p = new BatteryVoltageDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }
            }
            return (T)Activator.CreateInstance(typeof(T));

        }

        private T GetPumpStationSensor<T>(PumpStation pumpStation, ref UnderThresold model) where T : Sensor
        {
            foreach (var sensor in pumpStation.Sensors)
            {
                if (sensor is WMS.QueryModel.Sensors.PressureSensor && model.TransmeType == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                {
                    Sensor p = new PressureSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is LevelSensor && model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                {
                    Sensor p = new LevelSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is EnergySensor && model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    Sensor p = new EnergySensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;
                }

                if (sensor is WMS.QueryModel.Sensors.FlowSensor && model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
                {
                    Sensor p = new FlowSensor() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }
                if (sensor is WMS.QueryModel.Sensors.ChlorinePresenceDetector && model.TransmeType == Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR)
                {
                    Sensor p = new ChlorinePresenceDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }

                if (sensor is WMS.QueryModel.Sensors.ACPresenceDetector && model.TransmeType == Sensor.TransmitterType.AC_PRESENCE_DETECTOR)
                {
                    Sensor p = new ACPresenceDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

                }

                if (sensor is WMS.QueryModel.Sensors.BatteryVoltageDetector && model.TransmeType == Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR)
                {
                    Sensor p = new BatteryVoltageDetector() { SensorID = sensor.SensorID, UUID = sensor.UUID, CurrentValue = sensor.CurrentValue, MinimumValue = sensor.MinimumValue };
                    model.Unit = sensor.UnitName;
                    return (T)p;

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
                    model.XaxisCategory[i] = model.ToDateTime.AddHours(i + 1).ToString();
                }
            }
            return avgValue;
        }

        private List<double> GetUnderThresoldData(ref DrillDown model, int sensorId)
        {
            List<double> avgValue = new List<double>();
            for (int i = 0; i <= 24; i++)
            {
                if (model.TransmeType != Sensor.TransmitterType.ENERGY_TRANSMITTER)
                {
                    avgValue.Add(GetTotalDataWithinTime(sensorId, model.ToDateTime.AddHours(i),
                    model.ToDateTime.AddHours(i + 1)));
                    model.XaxisCategory[i] = model.ToDateTime.AddHours(i + 1).ToString();
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
                double value = (double)GetCurrentDataWithinTime(sensorId,
                    model.ToDateTime.AddMinutes(i == 0 ? 0 : i + 5),
                    model.ToDateTime.AddMinutes(i == 0 ? 5 : i * 5));
                avgValue.Add(value);
                model.XaxisCategory[i] = model.ToDateTime.AddMinutes(i * 5).ToShortTimeString();

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
                    model.XaxisCategory[i] = model.ToDateTime.AddDays(i + 1).ToString();
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
                    model.XaxisCategory[i] = model.ToDateTime.AddDays(i).ToString();
                }
            }
            return avgValue;
        }
        private double GetTotalDataWithinTime(int sensorId, DateTime from, DateTime to)
        {
            List<SensorData> sensorDataList = Container.Instances<SensorData>()
                   .Where(x => (x.Sensor.SensorID == sensorId && x.LoggedAt >= from && x.LoggedAt <= to)).ToList();

            if (sensorDataList != null)
                return (double)sensorDataList.Sum(x => x.Value);
            else
                return 0;
        }
        private decimal GetCurrentDataWithinTime(int sensorId, DateTime from, DateTime to)
        {
            SensorData sensorData = Container.Instances<SensorData>()
                   .Where(x => (x.Sensor.SensorID == sensorId && x.LoggedAt >= from && x.LoggedAt <= to)).FirstOrDefault();

            if (sensorData != null)
                return (sensorData.Value);
            else
                return 0;
        }

        private List<SensorData> GetCurrentDataListWithinTime(Sensor sensor, DateTime from, DateTime to)
        {
            return Container.Instances<SensorData>()
                   .Where(x => (x.Sensor.SensorID == sensor.SensorID && x.LoggedAt >= from && x.LoggedAt <= to && (x.Value <= sensor.MinimumValue))).ToList();
        }
        #endregion
    }
}
