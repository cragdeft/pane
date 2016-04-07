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

namespace AplombTech.WMS.QueryModel.Repositories
{
    [DisplayName("Reports")]
    public class ReportRepository : AbstractFactoryAndRepository
    {
        public static void Menu(IMenu menu)
        {
            menu.AddAction("GoogleMap");
            menu.AddAction("ScadaMap");
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
            var zones = new ZoneGoogleMap();
            zones.Zones = Container.Instances<Zone>().ToList();

            return zones;
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
    }
}
