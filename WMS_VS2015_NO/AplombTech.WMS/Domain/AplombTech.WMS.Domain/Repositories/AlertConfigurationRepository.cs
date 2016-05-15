using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Alerts;
using NakedObjects.Menu;
using NakedObjects;

namespace AplombTech.WMS.Domain.Repositories
{
    [DisplayName("Alerts")]
    public class AlertConfigurationRepository : AbstractFactoryAndRepository
    {
        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddAlertReceipient");
            menu.AddAction("ShowAlertRecipients");
            //menu.CreateSubMenu("Zone")
            //    .AddAction("CreateZone")
            //    .AddAction("FindZone")
            //    .AddAction("AllZones");
            //menu.CreateSubMenu("DMA")
            //    .AddAction("FindDMA");
            //menu.CreateSubMenu("PumpStation")
            //    .AddAction("FindPumpStation");
        }
        public AlertRecipient AddAlertReceipient(string name, Designation designation, [Description("Example: +8801534567890")][RegEx(Validation = @"^(?:\+88|01)?\d{11}\r?$", Message = "Not a valid mobile no")]string mobileNo, [RegEx(Validation = @"^[\-\w\.]+@[\-\w\.]+\.[A-Za-z]+$", Message = "Not a valid email address")]string email, IEnumerable<AlertType> alertTypes)
        {
            AlertRecipient recipient  = Container.NewTransientInstance<AlertRecipient>();

            recipient.Name = name;
            recipient.Designation = designation;
            recipient.MobileNo = mobileNo;
            recipient.Email = email;

            foreach (AlertType type in alertTypes)
                recipient.AlertTypes.Add(type);

            Container.Persist(ref recipient);

            return recipient;
        }
        //[DisplayName("All Zones")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(false, "AlertName")]
        public IQueryable<AlertRecipient> ShowAlertRecipients()
        {
            return Container.Instances<AlertRecipient>();
        }
    }
}
