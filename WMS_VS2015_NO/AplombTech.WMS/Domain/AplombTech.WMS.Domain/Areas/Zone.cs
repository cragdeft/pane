using AplombTech.WMS.Domain.Shared;
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

namespace AplombTech.WMS.Domain.Areas
{
    public class Zone : Area
    {
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

        #region AddDMA (Action)

        [DisplayName("Add DMA")]
        public void AddDMA(string name)
        {
            DMA dma = Container.NewTransientInstance<DMA>();
            dma.Name = name;

            dma.Parent = this;

            Container.Persist(ref dma);
        }

        #endregion

        #region SetAddress (Action)
        [DisplayName("Set Address")]
        public void SetAddress(string street1, [Optionally] string street2, string zipCode, string zone, string city)
        {
            Address address = Container.NewTransientInstance<Address>();
            address.Street1 = street1;
            address.Street2 = street2;
            address.ZipCode = zipCode;
            address.Zone = zone;
            address.City = city;

            Container.Persist(ref address);
            this.Address = address;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddDMA");
            menu.AddAction("SetAddress");
        }
        #endregion
    }
}
