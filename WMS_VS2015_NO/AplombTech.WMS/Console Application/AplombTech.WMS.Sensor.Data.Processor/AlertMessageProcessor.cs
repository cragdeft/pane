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
                AlertConfigurationRepository repo = new AlertConfigurationRepository(uow.CurrentObjectContext);
                alertMessage = repo.GetMessageByAlertMessageTypeId(message.AlertMessageType);
                recipients = repo.GetReceipientsByAlertTypeId(message.AlertMessageType);
            }
            if (message is SensorAlertMessage)
            {
                HandleSensorMessage((SensorAlertMessage)message, alertMessage, recipients);
            }
            if (message is MotorAlertMessage)
            {
                HandleMotorMessage((MotorAlertMessage)message, alertMessage, recipients);
            }
        }

        private void HandleMotorMessage(MotorAlertMessage motorMessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                string[] messageList = alertMessage.Split('|');

                string message = motorMessage.MotorName + " " + messageList[0] + " " + motorMessage.PumpStationName + " is "+motorMessage.MotorStatus;
                AlertConfigurationRepository repo = new AlertConfigurationRepository(uow.CurrentObjectContext);


                foreach (AlertRecipient recipient in recipients)
                {

                    AlertLog log = new AlertLog
                    {
                        AlertGereratedObjectId = motorMessage.MotorId,
                        MessageDateTime = DateTime.Now,
                        AlertMessageType = (int)AlertType.AlertTypeEnum.OnOff
                    };

                    if (recipient.Email.Trim().Length > 0)
                    {
                        log.ReciverEmail = recipient.Email;
                        EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", "Pump Motor Is Off",
                            message);
                    }
                    if (recipient.MobileNo.Trim().Length > 0)
                    {
                        log.ReciverMobileNo = recipient.MobileNo;
                        SmsSender.SendSMS(recipient.MobileNo, message);
                    }
                    repo.SaveAlertLog(log);
                }

                uow.CurrentObjectContext.SaveChanges();
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
                case (int)AlertType.AlertTypeEnum.OnOff:
                    SendOnOffMessage(message, alertMessage, recipients);
                    return;
            }
        }
        private void SendOnOffMessage(SensorAlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = objmessage.SensorName + " " + messageList[0] + " " + objmessage.PumpStationName + messageList[1];

            SendSensorMessage(objmessage, recipients, AlertType.AlertTypeEnum.OnOff, "On Off",message);
        }
        private void SendDataMissingMessage(SensorAlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " " + messageList[1] + " " + objmessage.PumpStationName;

            SendSensorMessage(objmessage, recipients, AlertType.AlertTypeEnum.DataMissing, "Data Missing", message);
        }
        private void SendUnderThresholdMessage(SensorAlertMessage objmessage, string alertMessage, IList<AlertRecipient> recipients)
        {
            string[] messageList = alertMessage.Split('|');
            string message = messageList[0] + " " + objmessage.SensorName + " (Minimum value is " +
                             objmessage.MinimumValue + " BUT received value is " + objmessage.Value + ") " +
                             messageList[1] + " " + objmessage.PumpStationName;

            SendSensorMessage(objmessage, recipients, AlertType.AlertTypeEnum.UnderThreshold, "Under threshold Data", message);
        }

        private void SendSensorMessage(SensorAlertMessage objmessage, IList<AlertRecipient> recipients, AlertType.AlertTypeEnum alertType, string mailSubject, string message)
        {
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                AlertConfigurationRepository repo = new AlertConfigurationRepository(uow.CurrentObjectContext);

                foreach (AlertRecipient recipient in recipients)
                {
                    var alertLog = repo.GetAlertLog(objmessage.SensorId, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour);
                    if (alertLog == null)
                    {
                        AlertLog log = new AlertLog
                        {
                            AlertGereratedObjectId = objmessage.SensorId,
                            MessageDateTime = DateTime.Now,
                            AlertMessageType = (int)alertType
                        };

                        if (recipient.Email.Trim().Length > 0)
                        {
                            log.ReciverEmail = recipient.Email;
                            EmailSender.SendEmail(recipient.Email, "mosharraf.hossain@aplombtechbd.com", mailSubject,
                               message);
                        }
                        if (recipient.MobileNo.Trim().Length > 0)
                        {
                            log.ReciverMobileNo = recipient.MobileNo;
                            SmsSender.SendSMS(recipient.MobileNo, message);
                        }
                        repo.SaveAlertLog(log);
                    }
                }
                uow.CurrentObjectContext.SaveChanges();
            }
        }

    }
}
