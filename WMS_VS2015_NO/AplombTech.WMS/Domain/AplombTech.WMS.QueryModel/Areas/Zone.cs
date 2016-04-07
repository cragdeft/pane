using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Areas
{
    public class Zone : Area
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Show DMA
        [MemberOrder(20), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("DMA")]
        [TableView(true, "Name")]
        public IList<DMA> DMAs
        {
            get
            {
                IList<DMA> dmas = (from dma in Container.Instances<DMA>()
                                   where dma.Parent.AreaID == this.AreaID
                                   select dma).ToList();
                return dmas;
            }
        }
        #endregion
    }
}
