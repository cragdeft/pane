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
            var zones = Container.NewViewModel<ScadaMap>();
            zones.Zones = Container.Instances<Zone>().ToList();

            return zones;
        }
    }
}
