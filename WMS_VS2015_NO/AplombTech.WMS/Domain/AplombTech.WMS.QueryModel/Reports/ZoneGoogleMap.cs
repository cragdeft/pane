﻿using AplombTech.WMS.QueryModel.Areas;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Reports
{
    [NotMapped]
    public class ZoneGoogleMap : IViewModel    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Google Map";

            t.Append(title);

            return t.ToString();
        }
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        [DisplayName("Zone Map")]
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
