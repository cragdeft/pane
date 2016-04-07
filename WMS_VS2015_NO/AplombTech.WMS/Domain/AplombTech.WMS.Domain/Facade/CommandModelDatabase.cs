using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Facade
{
    public class CommandModelDatabase : DbContext 
    {
        public CommandModelDatabase(string name) : base(name) { }
        public CommandModelDatabase() { }

        //Add DbSet properties for root objects, thus:
        public DbSet<Area> Areas { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Router> Routers { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<LevelSensor> LevelSensors { get; set; }
        public DbSet<FlowSensor> FlowSensors { get; set; }
        public DbSet<EnergySensor> EnergySensors { get; set; }
        public DbSet<ChlorinationSensor> ChlorinationSensors { get; set; }
        public DbSet<PressureSensor> PressureSensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<SensorDataLog> SensorDataLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Initialisation
            //Use the Naked Objects > DbInitialiser template to add an initialiser, then reference thus:
#if DEBUG

            Database.SetInitializer(new SampleAppInitializer());
#else
            Database.SetInitializer(null);
#endif

            //Mappings
            //Use the Naked Objects > DbMapping template to create mapping classes & reference them thus:
            //modelBuilder.Configurations.Add(new EmployeeMapping());
            //modelBuilder.Configurations.Add(new CustomerMapping());

            //modelBuilder.Entity<Asar>()
            //.Property(f => f.DateOfEstablishment)
            //.HasColumnType("datetime2");

            //modelBuilder.Properties<DateTime>()
            //.Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<SensorData>().Property(sd => sd.Value).HasPrecision(18, 2);
            modelBuilder.Entity<Sensor>().Property(sensor => sensor.MinimumValue).HasPrecision(18, 2);
            modelBuilder.Entity<Sensor>().Property(sensor => sensor.MaximumValue).HasPrecision(18, 2);
            modelBuilder.Entity<Sensor>().Property(sensor => sensor.CurrentValue).HasPrecision(18, 2);
            modelBuilder.Entity<EnergySensor>().Property(sensor => sensor.CumulativeValue).HasPrecision(18, 2);
            modelBuilder.Entity<FlowSensor>().Property(sensor => sensor.CumulativeValue).HasPrecision(18, 2);
            modelBuilder.Entity<Pump>().Property(pump => pump.Capacity).HasPrecision(18, 2);
        }
    }
}
