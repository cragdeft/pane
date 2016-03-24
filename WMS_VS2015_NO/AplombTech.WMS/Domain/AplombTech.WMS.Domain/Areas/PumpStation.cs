using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Areas
{
    public class PumpStation : Area
    {
        #region Get Properties
        
        [MemberOrder(50), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Pump")]
        [TableView(true, "UUID", "ModelNo")]
        public IList<Pump> Pumps
        {
            get
            {
                IList<Pump> pumps = (from pump in Container.Instances<Pump>()
                                    where pump.PumpStation.AreaID == this.AreaID
                                    select pump).ToList();
                return pumps;
            }
        }

        [MemberOrder(60), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Camera")]
        [TableView(true, "UUID", "URL")]
        public IList<Camera> Cameras
        {
            get
            {
                IList<Camera> cameras = (from camera in Container.Instances<Camera>()
                                         where camera.PumpStation.AreaID == this.AreaID
                                         select camera).ToList();
                return cameras;
            }
        }

        [MemberOrder(70), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Router")]
        [TableView(true, "UUID", "MACAddress", "IP", "Port")]
        public IList<Router> Routers
        {
            get
            {
                IList<Router> routers = (from router in Container.Instances<Router>()
                                         where router.PumpStation.AreaID == this.AreaID
                                         select router).ToList();
                return routers;
            }
        }

        [MemberOrder(80), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Sensors")]
        [TableView(true, "UUID", "MinimumValue", "MaximumValue")]
        public IList<Sensor> Sensors
        {
            get
            {
                IList<Sensor> sensors = (from sensor in Container.Instances<Sensor>()
                                         where sensor.PumpStation.AreaID == this.AreaID
                                         select sensor).ToList();
                return sensors;
            }
        }
        
        #endregion

        #region AddPump (Action)

        [DisplayName("Add Pump")]
        public void AddPump(string modelNo, string uuid)
        {
            Pump pump = Container.NewTransientInstance<Pump>();
            pump.ModelNo = modelNo;
            pump.UUID = uuid;

            pump.PumpStation = this;

            Container.Persist(ref pump);
        }

        #endregion

        #region AddCamera (Action)

        [DisplayName("Add Camera")]
        public void AddCamera(string url, string uuid)
        {
            Camera camera = Container.NewTransientInstance<Camera>();
            camera.URL = url;
            camera.UUID = uuid;

            camera.PumpStation = this;

            Container.Persist(ref camera);
        }

        #endregion

        #region AddRouter (Action)

        [DisplayName("Add Router")]
        public void AddRouter(string uuid, string ip, int port)
        {
            Router router = Container.NewTransientInstance<Router>();
            router.UUID = uuid;
            router.IP = ip;
            router.Port = port;

            router.PumpStation = this;

            Container.Persist(ref router);
        }

        #endregion

        #region AddSensor (Action)

        [DisplayName("Add Sensor")]
        public void AddSensor([Required]Sensor.TransmitterType sensorType, string uuid, decimal minValue, decimal maxValue)
        {
            Sensor sensor = Container.NewTransientInstance<Sensor>();
            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.SensorType = sensorType;
            sensor.PumpStation = this;

            Container.Persist(ref sensor);
        }

        #endregion

        #region SetAddress (Action)
        [DisplayName("Set Address")]
        public void SetAddress([Required]string street1, string street2, string zipCode, string zone, string city)
        {
            Address address = Container.NewTransientInstance<Address>();
            address.Street1 = street1;
            address.Street2 = street2;
            address.ZipCode = zipCode;
            address.Zone = zone;
            address.City = city;

            Container.Persist(ref address);
            this.Address = address;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            //
            var sub = menu.CreateSubMenu("Device");
            sub.AddAction("AddPump");
            sub.AddAction("AddCamera");
            sub.AddAction("AddRouter");
            sub.AddAction("AddSensor");

            menu.AddAction("SetAddress");

            //sub = menu.CreateSubMenu("সভ্য");
            //sub.AddAction("AddMember");
            //sub.AddAction("AllMembers");

            //sub = menu.CreateSubMenu("কর্মী");
            //sub.AddAction("AddKormi");
            //sub.AddAction("ShowAllKormi");

            //sub = menu.CreateSubMenu("কমিটি");
            //sub.AddAction("NewCommittee");
            //sub.AddAction("ShowCurrentCommittee");
            //sub.AddAction("ShowAllCommittees");

            //sub = menu.CreateSubMenu("টীকা");
            //sub.AddAction("NewNote");
            //sub.AddAction("ShowActivities");

            //menu.AddRemainingNativeActions();
            //menu.AddContributedActions();
        }
        #endregion

        public override Area Parent { get; set; }
        [PageSize(10)]
        public IQueryable<DMA> AutoCompleteParent([MinLength(3)] string name)
        {
            return AreaRepository.FindDMA(name);
        }
    }
}
