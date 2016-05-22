using AplombTech.WMS.QueryModel.Areas;
using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Features;
using AplombTech.WMS.QueryModel.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using AplombTech.WMS.QueryModel.UserAccounts;
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
            Areas = base.Set<Area>();
            Devices = base.Set<Device>();
            Pumps = base.Set<Pump>();
            Cameras = base.Set<Camera>();
            Routers = base.Set<Router>();
            Sensors = base.Set<Sensor>();
            SensorDatas = base.Set<SensorData>();
            Addresses = base.Set<Address>();
            Roles = base.Set<Role>();
            LoginUsers = base.Set<LoginUser>();
            UserRoles = base.Set<UserRoles>();
            FeatureTypes = base.Set<FeatureType>();
            Features = base.Set<Feature>();
            RoleFeatures = base.Set<RoleFeatures>();
        }

        public DbSet<Area> Areas { get; private set; }
        public DbSet<Device> Devices { get; private set; }
        public DbSet<Pump> Pumps { get; private set; }
        public DbSet<Camera> Cameras { get; private set; }
        public DbSet<Router> Routers { get; private set; }
        public DbSet<Sensor> Sensors { get; private set; }
        public DbSet<SensorData> SensorDatas { get; private set; }
        public DbSet<Address> Addresses { get; private set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Role> Roles { get; private set; }
        public DbSet<LoginUser> LoginUsers { get; private set; }
        public DbSet<UserRoles> UserRoles { get; private set; }
        //public DbSet<UserLogins> UserLogins { get; set; }
        //public DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; private set; }
        public DbSet<Feature> Features { get; private set; }
        public DbSet<RoleFeatures> RoleFeatures { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Initialisation
            //Use the Naked Objects > DbInitialiser template to add an initialiser, then reference thus:
           
            Database.SetInitializer<QueryModelDatabase>(null);

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
