using AplombTech.WMS.DataLayer;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.AreaBoundedContext
{
    public class AreaContext : BaseContext<AreaContext>
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Router> Routers { get; set; }
        public DbSet<Motor> Motors { get; set; }
        public DbSet<PumpMotor> PumpMotors { get; set; }
        public DbSet<ChlorineMotor> ChlorineMotors { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<LevelSensor> LevelSensors { get; set; }
        public DbSet<FlowSensor> FlowSensors { get; set; }
        public DbSet<EnergySensor> EnergySensors { get; set; }
        public DbSet<ChlorinePresenceDetector> ChlorinePresenceDetectors { get; set; }
        public DbSet<ACPresenceDetector> ACPresenceDetectors { get; set; }
        public DbSet<BatteryVoltageDetector> BatteryVoltageDetectors { get; set; }
        public DbSet<PressureSensor> PressureSensors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AreaContext>(null);
        }
    }
}
