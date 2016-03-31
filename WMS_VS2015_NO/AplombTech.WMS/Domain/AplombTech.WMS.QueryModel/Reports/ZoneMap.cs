using AplombTech.WMS.QueryModel.Areas;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Reports
{
    [NotMapped]
    public class ZoneMap : IViewModel    {
        public IDomainObjectContainer Container { set; protected get; }  //Injected service

        public virtual IList<Zone> Zones { get; set; }
        public string[] DeriveKeys()
        {
            string[] ids = Zones.Select(s => s.AreaID.ToString()).ToArray();
            return ids;
        }
        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            Zones = Container.Instances<Zone>().Where(w => ids.Contains(w.AreaID.ToString())).ToList();
        }
    }
}
