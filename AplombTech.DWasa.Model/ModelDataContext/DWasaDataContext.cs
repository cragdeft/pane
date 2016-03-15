using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Model.Models;
using Repository.Pattern.Ef6;

namespace AplombTech.DWasa.Model.ModelDataContext
{
    public partial class DWasaDataContext : DataContext, IDWasaDataContext
    {
        static DWasaDataContext()
        {
            Database.SetInitializer<DWasaDataContext>(null);
        }
        public DWasaDataContext()
            : base("DWasa")
        {

        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
        public IDbSet<Zone> Zones { get; set; }

        public IDbSet<DMA> DMAs { get; set; }

        public IDbSet<PumpStation> PumpStations { get; set; }

        public IDbSet<Camera> Cameras { get; set; }

        public IDbSet<Router> Routers { get; set; }

        public IDbSet<Sensor> Sensors { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DWasaDataContext>());
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            //base.Database.ExecuteSqlCommand(command, parameters);
        }
    }
}
