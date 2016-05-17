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
                SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", "Data Missing", message);
                SendSMS(recipient.MobileNo, message);
            }
        }
        private void SendUnderThresholdMessage(AlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " (Minimum value is " + objmessage.MinimumValue + " BUT received value is " + objmessage.Value + ") " + messageList[1] + " " + objmessage.PumpStationName;

            foreach (AlertRecipient recipient in recipients)
            {
                SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com","Under threshold Data", message);
                SendSMS(recipient.MobileNo, message);
            }
        }
        private string SendEmail(string to, string from, string subject, string body)
        {
            try
            {
                var httpReq = (HttpWebRequest)WebRequest.Create("http://emailservice.azurewebsites.net/EmailService.svc/SendEmailMessage?to=" + to + "&from=" + from + "&subject=" + subject + "&body=" + body);
                httpReq.Method = "POST";
                httpReq.ContentType = "application/x-www-form-urlencoded";
                httpReq.ContentLength = 0;

                var response = (HttpWebResponse)httpReq.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (responseString.Contains("Success"))
                {
                    return "Email Sent";
                }
                else
                {
                    return "Email Failed";
                }
            }
            catch (Exception ex)
            {
                return "Email Failed";
            }
        }
        private void SendSMS(string mobileno, string message)
        {
            
        }
    }
}
