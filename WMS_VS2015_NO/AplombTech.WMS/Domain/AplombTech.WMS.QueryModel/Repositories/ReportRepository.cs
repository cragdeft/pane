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
    }
}
