﻿using AplombTech.WMS.Domain.Devices;
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
        public override string Name { get; set; }

        #region Validations
        public string ValidateName(string areaName)
        {
            var rb = new ReasonBuilder();

            PumpStation station = (from obj in Container.Instances<PumpStation>()
                                   where obj.Name == areaName
                                   select obj).FirstOrDefault();

            if (station != null)
            {
                if (this.AreaID != station.AreaID)
                {
                    rb.AppendOnCondition(true, "Duplicate PumpStation Name");
                }
            }
            return rb.Reason;
        }
        #endregion

        #region Get Properties      
        [MemberOrder(50), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Pump")]
        [TableView(true, "UUID", "ModelNo")]
        public Pump Pump
        {
            get
            {
                Pump pumps = (from pump in Container.Instances<Pump>()
                              where pump.PumpStation.AreaID == this.AreaID
                              && pump.IsRemoved ==false
                              select pump).FirstOrDefault();
                return pumps;
            }
        }
        
        [MemberOrder(60), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Router")]
        [TableView(true, "UUID", "MACAddress", "IP", "Port")]
        public Router Router
        {
            get
            {
                Router router = (from r in Container.Instances<Router>()
                                 where r.PumpStation.AreaID == this.AreaID
                                 select r).FirstOrDefault();
                return router;
            }
        }

        [MemberOrder(70), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Sensors")]
        [TableView(true, "CurrentValue", "CumulativeValue")]
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

        [MemberOrder(80), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
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
        #endregion

        #region AddPump (Action)
        [DisplayName("Add Pump")]
        public void AddPump(string modelNo, string uuid)
        {
            Pump pump = Container.NewTransientInstance<Pump>();
            pump.ModelNo = modelNo;
            pump.UUID = uuid;
            pump.IsRemoved = false;

            pump.PumpStation = this;

            Container.Persist(ref pump);
        }
        public bool HideAddPump()
        {
            if (this.Pump != null)
                return true;

            return false;
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
        public bool HideAddRouter()
        {
            if (this.Router != null)
                return true;

            return false;
        }
        #endregion

        #region AddSensor (Action)

        [DisplayName("Add Sensor")]
        public void AddSensor([Required]Sensor.TransmitterType sensorType, string uuid, decimal minValue, decimal maxValue)
        {
            switch (sensorType)
            {
                case Sensor.TransmitterType.CHLORINE_TRANSMITTER:
                    CreateChlorinationSensor(uuid, minValue, maxValue);
                    break;

                case Sensor.TransmitterType.ENERGY_TRANSMITTER:
                    CreateEnergySensor(uuid, minValue, maxValue);
                    break;

                case Sensor.TransmitterType.FLOW_TRANSMITTER:
                    CreateFlowSensor(uuid, minValue, maxValue);
                    break;

                case Sensor.TransmitterType.LEVEL_TRANSMITTER:
                    CreateLevelSensor(uuid, minValue, maxValue);
                    break;

                case Sensor.TransmitterType.PRESSURE_TRANSMITTER:
                    CreatePressureSensor(uuid, minValue, maxValue);
                    break;
            }
        }

        private void CreateChlorinationSensor(string uuid, decimal minValue, decimal maxValue)
        {
            ChlorinationSensor sensor = Container.NewTransientInstance<ChlorinationSensor>();

            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.CurrentValue = 0;
            //sensor.CumulativeValue = 0;
            sensor.PumpStation = this;

            Container.Persist(ref sensor);
        }
        private void CreateFlowSensor(string uuid, decimal minValue, decimal maxValue)
        {
            FlowSensor sensor = Container.NewTransientInstance<FlowSensor>();

            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.CurrentValue = 0;
            sensor.CumulativeValue = 0;
            sensor.PumpStation = this;

            Container.Persist(ref sensor);
        }
        private void CreateEnergySensor(string uuid, decimal minValue, decimal maxValue)
        {
            EnergySensor sensor = Container.NewTransientInstance<EnergySensor>();

            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.CurrentValue = 0;
            sensor.CumulativeValue = 0;
            sensor.PumpStation = this;

            Container.Persist(ref sensor);
        }
        private void CreateLevelSensor(string uuid, decimal minValue, decimal maxValue)
        {
            LevelSensor sensor = Container.NewTransientInstance<LevelSensor>();

            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.CurrentValue = 0;
            sensor.PumpStation = this;

            Container.Persist(ref sensor);
        }
        private void CreatePressureSensor(string uuid, decimal minValue, decimal maxValue)
        {
            PressureSensor sensor = Container.NewTransientInstance<PressureSensor>();

            sensor.UUID = uuid;
            sensor.MinimumValue = minValue;
            sensor.MaximumValue = maxValue;
            sensor.CurrentValue = 0;
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
        public bool HideSetAddress()
        {
            if (this.Address != null)
                return true;

            return false;
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
