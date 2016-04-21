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
using NakedObjects.Value;

namespace AplombTech.WMS.Domain.Areas
{   
    public class Zone : Area
    {
        public override string Name { get; set; }

        public virtual FileAttachment Kml
        {
            get
            {
                if (AttContent == null) return null;
                return new FileAttachment(AttContent, AttName, AttMime) { DispositionType = "inline" };
            }
        }

        [NakedObjectsIgnore]
        public virtual byte[] AttContent { get; set; }

        [NakedObjectsIgnore]
        public virtual string AttName { get; set; }

        [NakedObjectsIgnore]
        public virtual string AttMime { get; set; }

        #region Kml File Add
        public void AddOrChangeAttachment(FileAttachment newAttachment)
        {
            AttContent = newAttachment.GetResourceAsByteArray();
            AttName = newAttachment.Name;
            AttMime = newAttachment.MimeType;
        } 
        #endregion

        #region Validations
        public string ValidateName(string areaName)
        {
            var rb = new ReasonBuilder();

            Zone zone = (from obj in Container.Instances<Zone>()
                         where obj.Name == areaName
                         select obj).FirstOrDefault();

            if (zone != null)
            {
                if (this.AreaID != zone.AreaID)
                {
                    rb.AppendOnCondition(true, "Duplicate Zone Name");
                }
            }
            return rb.Reason;
        }
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

        #region AddDMA (Action)
        [DisplayName("Add DMA")]
        public void AddDMA(string name)
        {
            DMA dma = Container.NewTransientInstance<DMA>();
            dma.Name = name;

            dma.Parent = this;

            Container.Persist(ref dma);
        }

        #region Validations
        public string ValidateAddDMA(string name)
        {
            var rb = new ReasonBuilder();

            DMA dma = (from obj in Container.Instances<DMA>()
                       where obj.Name == name
                       select obj).FirstOrDefault();

            if (dma != null)
            {
                rb.AppendOnCondition(true, "Duplicate DMA Name");
            }
            return rb.Reason;
        }
        #endregion
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
        public bool HideSetAddress()
        {
            if(this.Address != null)
                return true;

            return false;
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
