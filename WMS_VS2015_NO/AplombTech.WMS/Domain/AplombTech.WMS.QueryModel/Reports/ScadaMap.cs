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
    public class ScadaMap : IViewModel
    {
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        [Title, DisplayName("Scada Map")]
        public virtual IList<Zone> Zones { get; set; }
        public virtual IList<DMA> Dmas { get; set; }
        public virtual IList<PumpStation> PumpStations { get; set; }
        public string[] DeriveKeys()
        {
            string[] ids = PumpStations.Select(s => s.AreaID.ToString()).ToArray();
            return ids;
        }
        public int SelectedZoneId { get; set; }
        public int SelectedDmaId { get; set; }
        public int SelectedPumpStationId { get; set; }
        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            Zones = Container.Instances<Zone>().Where(w => ids.Contains(w.AreaID.ToString())).ToList();
            Dmas = Container.Instances<DMA>().Where(w => ids.Contains(w.AreaID.ToString())).ToList();
            PumpStations = Container.Instances<PumpStation>().Where(w => ids.Contains(w.AreaID.ToString())).ToList();
        }
    }
}
