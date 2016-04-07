using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Shared;
using NakedObjects.Menu;

namespace AplombTech.WMS.Domain.Areas
{
    public class DMA : Area
    {
        public override string Name { get; set; }

        #region Validations
        public string ValidateName(string areaName)
        {
            var rb = new ReasonBuilder();

            DMA dma = (from obj in Container.Instances<DMA>()
                       where obj.Name == areaName
                       select obj).FirstOrDefault();

            if (dma != null)
            {
                if (this.AreaID != dma.AreaID)
                {
                    rb.AppendOnCondition(true, "Duplicate DMA Name");
                }
            }
            return rb.Reason;
        }
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

        #region AddPumpStation (Action)

        [DisplayName("Add PumpStation")]
        public void AddPumpStation(string name)
        {
            PumpStation station = Container.NewTransientInstance<PumpStation>();
            station.Name = name;

            station.Parent = this;

            Container.Persist(ref station);
        }
        #region Validations
        public string ValidateAddPumpStation(string name)
        {
            var rb = new ReasonBuilder();

            PumpStation station = (from obj in Container.Instances<PumpStation>()
                                   where obj.Name == name
                                   select obj).FirstOrDefault();

            if (station != null)
            {
                rb.AppendOnCondition(true, "Duplicate DMA Name");
            }
            return rb.Reason;
        }
        #endregion
        #endregion

        #region SetAddress (Action)
        [DisplayName("Set Address")]
        public void SetAddress([Required]string street1, string street2, string zipCode, string zone, string city)
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
        public bool HideSetAddress()
        {
            if (this.Address != null)
                return true;

            return false;
        }
        #endregion

        #region Menu

        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddPumpStation");
            menu.AddAction("SetAddress");
        }

        #endregion

        public override Area Parent { get; set; }
        [PageSize(10)]
        public IQueryable<Zone> AutoCompleteParent([MinLength(3)] string name)
        {
            return AreaRepository.FindZone(name);
        }
    }
}
