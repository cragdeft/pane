using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using NakedObjects.Async;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client
{
    public class MqttClientService: AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        public IAsyncService AsyncService { private get; set; }
        #endregion

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MqttClientWrapper instance = null;
        //Lock synchronization object
        private object syncLock = new object();
        public void MQTTClientInstance(bool isSSL)
        {
            lock (syncLock)
            {
                if (instance == null)
                {
                    instance = new MqttClientWrapper(isSSL);
                    
                    instance.NotifyMqttMessageReceivedEvent += new MqttClientWrapper.NotifyMqttMessageReceivedDelegate(PublishReceivedMessage_NotifyEvent);

                    instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_NotifyEvent);

                    instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_NotifyEvent);
                    instance.MakeConnection();
                }

                //return instance;
            }
        }
        private void PublishReceivedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {            
            log.Info("Message Received from " + customEventArgs.ReceivedTopic + " Topic");
            SensorDataLog dataLog = ProcessRepository.LogSensorData(customEventArgs.ReceivedTopic, customEventArgs.ReceivedMessage);

            if(dataLog == null)
            {
                instance.Publish("/ConfigFeedback", "Logged Date & Time missing");
                return;
            }
            instance.Publish("/ConfigFeedback", "Message has been logged Sucessfully");
                                    
            if (customEventArgs.ReceivedTopic == "/configuration")
            {
                if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
                {
                    AsyncService.RunAsync((domainObjectContainer) =>
                             ProcessRepository.ParseNStoreSensorData(dataLog));
                }
            }
            //if (customEventArgs.ReceivedTopic == CommandType.Configuration.ToString())
            //{
            //    new JsonManager().JsonProcess(customEventArgs.ReceivedMessage);
            //}
        }
      
        private void PublishedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;

        }
        private void SubscribedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;
        }
    }
}
