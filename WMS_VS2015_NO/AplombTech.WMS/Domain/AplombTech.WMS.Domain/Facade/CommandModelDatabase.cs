﻿using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.UserAccounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.SummaryData;

namespace AplombTech.WMS.Domain.Facade
{
    public class CommandModelDatabase : DbContext 
    {
        public CommandModelDatabase(string name) : base(name) { }
        public CommandModelDatabase() { }

        //Add DbSet properties for root objects, thus:
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
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<MotorData> MotorDatas { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<DataLog> SensorDataLogs { get; set; }
        public DbSet<AlertType> AlertTypes { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<AlertRecipient> AlertRecipients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserLogins> UserLogins { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<RoleFeatures> RoleFeatures { get; set; }
        public DbSet<SensorSummaryDataHourly> SensorSummaryDataHourly { get; set; }
        public DbSet<SensorSummaryDataDaily> SensorSummaryDataDaily { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #if DEBUG
                Database.SetInitializer(new SampleAppInitializer());
            #else
                Database.SetInitializer<CommandModelDatabase>(null);
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

            //modelBuilder.Entity<AlertRecipient>()
            //    .HasMany<AlertType>(s => s.AlertTypes)
            //    .WithMany(c => c.AlertRecipients)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("StudentRefId");
            //        cs.MapRightKey("CourseRefId");
            //        cs.ToTable("StudentCourse");
            //    });
            
            modelBuilder.Entity<Sensor>().Property(sensor => sensor.MinimumValue).HasPrecision(18, 2);
            modelBuilder.Entity<Sensor>().Property(sensor => sensor.MaximumValue).HasPrecision(18, 2);
            modelBuilder.Entity<Motors.PumpMotor>().Property(pump => pump.Capacity).HasPrecision(18, 2);
        }
    }
}
