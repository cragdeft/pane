using AplombTech.MQTTLib;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using NakedObjects;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.MQTTService
{
    public class MqttClientFacade: AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

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
            IList<Zone> zones = AreaRepository.AllZones().ToList();
#if DEBUG
            Debug.WriteLine(customEventArgs.ReceivedTopic);
#endif
            instance.Publish("/topic", "Message Received with Thanks");
            if (customEventArgs.ReceivedTopic == "/configuration")
            {
                string msg = customEventArgs.ReceivedMessage;
                int count = zones.Count();
#if DEBUG
                Debug.WriteLine(msg);
                Debug.WriteLine(count);
#endif
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
