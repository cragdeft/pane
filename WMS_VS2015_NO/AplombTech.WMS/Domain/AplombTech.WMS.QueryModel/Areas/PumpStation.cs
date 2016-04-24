using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Sensors;
using NakedObjects;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AplombTech.WMS.QueryModel.Areas
{
    public class PumpStation : Area
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Get Properties      
        [MemberOrder(50), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Pump")]
        [TableView(true, "UUID", "ModelNo")]
        public Pump Pumps
        {
            get
            {
                Pump pumps = (from pump in Container.Instances<Pump>()
                              where pump.PumpStation.AreaID == this.AreaID
                              select pump).FirstOrDefault();
                return pumps;
            }
        }

        [MemberOrder(60), NotMapped]
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

        [MemberOrder(70), NotMapped]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
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
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Sensors")]
        [TableView(true, "CurrentValue")]
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
    }
}
