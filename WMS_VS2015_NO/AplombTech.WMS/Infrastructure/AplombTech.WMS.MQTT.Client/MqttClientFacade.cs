using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.MQTT.Client
{
    public class MqttClientFacade
    {
        private static MqttClientWrapper instance = null;
        //Lock synchronization object
        private static object syncLock = new object();
        public static MqttClientWrapper MQTTClientInstance
        {
            get
            {
                lock (syncLock)
                {
                    if (MqttClientFacade.instance == null)
                    {
                        instance = new MqttClientWrapper(false);
                        instance.NotifyMqttMessageReceivedEvent += new MqttClientWrapper.NotifyMqttMessageReceivedDelegate(PublishReceivedMessage_NotifyEvent);

                        instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_NotifyEvent);

                        instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_NotifyEvent);
                    }

                    return instance;
                }
            }
        }
        static void PublishReceivedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            //if (customEventArgs.ReceivedTopic == CommandType.Configuration.ToString())
            //{
            //    new JsonManager().JsonProcess(customEventArgs.ReceivedMessage);
            //}
        }
        static void PublishedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;

        }
        static void SubscribedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;
        }
    }
}
