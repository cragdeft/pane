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

        public enum JsonMessageType
        {
            configuration,
            sensordata,
            feedback
        }

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
                    
                    instance.NotifyMqttMessageReceivedEvent += new MqttClientWrapper.NotifyMqttMessageReceivedDelegate(ReceivedMessage_MQTT);

                    instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_MQTT);

                    instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_MQTT);
                    instance.MakeConnection();
                }

                //return instance;
            }
        }
        private void ReceivedMessage_MQTT(MQTTEventArgs customEventArgs)
        {
            log.Info("Message received from topic '" + customEventArgs.ReceivedTopic + "' and message is '" + customEventArgs.ReceivedMessage + "'");
            AsyncService.RunAsync((domainObjectContainer) =>
                             ProcessMessage(customEventArgs));
        }
      
        private void PublishedMessage_MQTT(MQTTEventArgs customEventArgs)
        {
            //string msg = customEventArgs.ReceivedMessage;
            log.Info("Message published to '" + customEventArgs.ReceivedTopic + "' Topic");            
        }
        private void SubscribedMessage_MQTT(MQTTEventArgs customEventArgs)
        {
            //string msg = customEventArgs.ReceivedMessage;
        }

        private void ProcessMessage(MQTTEventArgs customEventArgs)
        {
            try
            {
                SensorDataLog dataLog = ProcessRepository.LogSensorData(customEventArgs.ReceivedTopic, customEventArgs.ReceivedMessage);

                if (dataLog == null)
                {
                    instance.Publish(customEventArgs.ReceivedTopic + JsonMessageType.feedback.ToString(), "Logged Date & Time is missing");
                    return;
                }
                instance.Publish(customEventArgs.ReceivedTopic + JsonMessageType.feedback.ToString(), "Message has been logged Sucessfully");

                if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
                {
                    if (customEventArgs.ReceivedTopic == JsonMessageType.sensordata.ToString())
                    {
                        ProcessRepository.ParseNStoreSensorData(dataLog);
                    }
                    if (customEventArgs.ReceivedTopic == JsonMessageType.configuration.ToString())
                    {
                        ProcessRepository.ParseNStoreConfigurationData(dataLog);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
            }
        }
    }
}
