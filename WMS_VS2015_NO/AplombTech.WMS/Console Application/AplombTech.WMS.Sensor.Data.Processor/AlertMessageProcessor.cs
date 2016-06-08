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
            if (message is SensorAlertMessage)
            {
                HandleSensorMessage((SensorAlertMessage) message, alertMessage, recipients);
            }
            if (message is MotorAlertMessage)
            {
                HandleMotorMessage((MotorAlertMessage)message, alertMessage, recipients);
            }
        }

        private void HandleMotorMessage(MotorAlertMessage motorMessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = motorMessage.MotorName + " " + messageList[0] + " " + motorMessage.PumpStationName + messageList[1];

            foreach (AlertRecipient recipient in recipients)
            {
                if (recipient.Email.Trim().Length > 0)
                    EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", "Pump Motor Is Off", message);

                SmsSender.SendSMS(recipient.MobileNo, message);
            }
        }

        private void HandleSensorMessage(SensorAlertMessage message, string alertMessage, IList<AlertRecipient> recipients)
        {
            switch (message.AlertMessageType)
            {
                case (int)AlertType.AlertTypeEnum.DataMissing:
                    SendDataMissingMessage(message, alertMessage, recipients);
                    return;
                case (int)AlertType.AlertTypeEnum.UnderThreshold:
                    SendUnderThresholdMessage(message, alertMessage, recipients);
                    return;
            }
        }
        private void SendDataMissingMessage(SensorAlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " " + messageList[1] + " " + objmessage.PumpStationName ;

            foreach (AlertRecipient recipient in recipients)
            {
                if(recipient.Email.Trim().Length > 0)
                    EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", "Data Missing", message);

                SmsSender.SendSMS(recipient.MobileNo, message);
            }
        }
        private void SendUnderThresholdMessage(SensorAlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " (Minimum value is " + objmessage.MinimumValue + " BUT received value is " + objmessage.Value + ") " + messageList[1] + " " + objmessage.PumpStationName;

            foreach (AlertRecipient recipient in recipients)
            {
                if (recipient.Email.Trim().Length > 0)
                    EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com","Under threshold Data", message);
                SmsSender.SendSMS(recipient.MobileNo, message);
            }
        }
    }
}
