using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Repositories
{
    [DisplayName ("Area")]
    public class AreaRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public ProcessRepository ProcessRepository { set; protected get; }
        #endregion
        public static void Menu(IMenu menu)
        {
            //menu.AddAction("FindCustomerByAccountNumber");
            menu.CreateSubMenu("Zone")
                .AddAction("CreateZone")
                .AddAction("FindZone")
                .AddAction("AllZones");
            menu.CreateSubMenu("DMA")
                .AddAction("FindDMA");
            menu.CreateSubMenu("PumpStation")
                .AddAction("FindPumpStation");
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

        #region Zone
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Zone")]
        public Zone CreateZone([Required] string name)
        {
            Zone zone = Container.NewTransientInstance<Zone>();

            zone.Name = name;

            Container.Persist(ref zone);
            return zone;
        }

        #region Validations
        public string ValidateCreateZone(string name)
        {
            var rb = new ReasonBuilder();

            Zone zone = (from obj in Container.Instances<Zone>()
                         where obj.Name == name
                         select obj).FirstOrDefault();

            if (zone != null)
            {
                rb.AppendOnCondition(true, "Duplicate Zone Name");
            }
            return rb.Reason;
        }
        #endregion

        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find Zone")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Zone> FindZone(string name)
        {
            IQueryable<Zone> zones = (from z in Container.Instances<Zone>()
                                      where z.Name.Contains(name)
                                      select z).OrderBy(o => o.Name);

            return zones;
        }

        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("All Zones")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Zone> AllZones()
        {
            return Container.Instances<Zone>();
        }

        #endregion

        #region DMA
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find DMA")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<DMA> FindDMA(string name)
        {
            IQueryable<DMA> dmas = (from z in Container.Instances<DMA>()
                                    where z.Name.Contains(name)
                                    select z).OrderBy(o => o.Name);

            return dmas;
        }

        #endregion

        #region PumpStation
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find PumpStation")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<PumpStation> FindPumpStation(string name)
        {
            IQueryable<PumpStation> stations = (from z in Container.Instances<PumpStation>()
                                                where z.Name.Contains(name)
                                                select z).OrderBy(o => o.Name);

            return stations;
        }
        #endregion
        public Sensor FindSensorByUid(string uid)
        {
            Sensor station = Container.Instances<Sensor>().Where(w => w.UUID == uid).First();
            return station;
        }
    }
}
