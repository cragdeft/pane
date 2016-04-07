using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects.Menu;

namespace AplombTech.WMS.QueryModel.Areas
{
    public class DMA : Area
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion
        #region Show PumpStation
        [MemberOrder(20), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Pump Station")]
        [TableView(true, "Name")]
        public IList<PumpStation> PumpStations
        {
            get
            {
                IList<PumpStation> stations = (from station in Container.Instances<PumpStation>()
                                               where station.Parent.AreaID == this.AreaID
                                               select station).ToList();
                return stations;
            }
        }
        #endregion
    }
}
