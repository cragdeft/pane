using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Facade
{
    public class QueryModelDatabase: DbContext
    {
        public QueryModelDatabase() { }
        public QueryModelDatabase(string name) : base(name)
        {
            //_areas = base.Set<Area>();
            //_zones = base.Set<Zone>();
            //_dmas = base.Set<DMA>();
            //_pumpStations = base.Set<PumpStation>();
            //_pumps = base.Set<Pump>();
            //_cameras = base.Set<Camera>();
            //_routers = base.Set<Router>();
            //_sensors = base.Set<Sensor>();
            //_sensorDatas = base.Set<SensorData>();
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Router> Routers { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Initialisation
            //Use the Naked Objects > DbInitialiser template to add an initialiser, then reference thus:
           
            Database.SetInitializer<QueryModelDatabase>(null);

            //Database.SetInitializer(new KhelaGharAMSDbInitialiser());

            //Mappings
            //Use the Naked Objects > DbMapping template to create mapping classes & reference them thus:
            //modelBuilder.Configurations.Add(new EmployeeMapping());
            //modelBuilder.Configurations.Add(new CustomerMapping());

            //modelBuilder.Entity<Asar>()
            //.Property(f => f.DateOfEstablishment)
            //.HasColumnType("datetime2");

            //modelBuilder.Properties<DateTime>()
            //.Configure(c => c.HasColumnType("datetime2"));
        }

        //public IQueryable<Zone> Zones
        //{
        //    get { return this._zones; }
        //}
        //public IQueryable<DMA> DMAs
        //{
        //    get { return _dmas; }
        //}
        //public IQueryable<PumpStation> PumpStations
        //{
        //    get { return _pumpStations; }
        //}
        //public IQueryable<Pump> Pumps
        //{
        //    get { return _pumps; }
        //}
        //public IQueryable<Camera> Cameras
        //{
        //    get { return _cameras; }
        //}
        //public IQueryable<Router> Routers
        //{
        //    get { return _routers; }
        //}
        //public IQueryable<Sensor> Sensors
        //{
        //    get { return _sensors; }
        //}
    }
}
