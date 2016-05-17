using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Utility;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public class AlertMessageProcessor : IHandleMessages<AlertMessage>
    {
        public void Handle(AlertMessage message)
        {
            string alertMessage = String.Empty;
            IList<AlertRecipient> recipients = new List<AlertRecipient>();
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                AlertConfigurationRepository repo = new AlertConfigurationRepository(WMSUnitOfWork.CurrentObjectContext);
                alertMessage = repo.GetMessageByAlertMessageTypeId(message.AlertMessageType);
                recipients = repo.GetReceipientsByAlertTypeId(message.AlertMessageType);
            }

            switch (message.AlertMessageType)
            {
                case (int) AlertType.AlertTypeEnum.DataMissing:
                    SendDataMissingMessage(message, alertMessage, recipients);
                    return;
                case (int) AlertType.AlertTypeEnum.UnderThreshold:
                    SendUnderThresholdMessage(message, alertMessage, recipients);
                    return;
                case (int)AlertType.AlertTypeEnum.PumpOnOff:
                    return;
            }
        }

        private void SendDataMissingMessage(AlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " " + messageList[1] + " " + objmessage.PumpStationName ;

            foreach (AlertRecipient recipient in recipients)
            {
                EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", "Data Missing", message);
                SmsSender.SendSMS(recipient.MobileNo, message);
            }
        }
        private void SendUnderThresholdMessage(AlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " (Minimum value is " + objmessage.MinimumValue + " BUT received value is " + objmessage.Value + ") " + messageList[1] + " " + objmessage.PumpStationName;

            foreach (AlertRecipient recipient in recipients)
            {
                EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com","Under threshold Data", message);
                SmsSender.SendSMS(recipient.MobileNo, message);
            }
        }
    }
}
