using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public class AlertMessageProcessor : IHandleMessages<AlertMessage>
    {
        public void Handle(AlertMessage message)
        {
            switch (message.AlertMessageType)
            {
                case (int) AlertType.AlertTypeEnum.DataMissing:
                    SendDataMissingMessage(message);
                    return;
                case (int) AlertType.AlertTypeEnum.UnderThreshold:
                    return;
                case (int)AlertType.AlertTypeEnum.PumpOnOff:
                    return;

            }
        }

        private void SendDataMissingMessage(AlertMessage objmessage)
        {
            string alertMessage = String.Empty;
            IList<AlertRecipient> receipients = new List<AlertRecipient>();
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                AlertConfigurationRepository repo = new AlertConfigurationRepository(WMSUnitOfWork.CurrentObjectContext);
                alertMessage = repo.GetMessageByAlertMessageTypeId(objmessage.AlertMessageType);
                receipients = repo.GetReceipientsByAlertTypeId(objmessage.AlertMessageType);
            }

            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " " + messageList[1] + " " + objmessage.PumpStationName ;

            foreach (AlertRecipient recipient in receipients)
            {
                SendEmail(recipient.Email, "Data Missing", message);
                SendSMS(recipient.MobileNo, message);
            }
        }

        private void SendEmail(string to, string subject, string body)
        {
            
        }

        private void SendSMS(string mobileno, string message)
        {
            
        }
    }
}
