using AplombTech.DWasa.MQTT.Client;
using AplombTech.DWasa.Utility.Enums;
using AplombTech.DWasa.Json;

namespace AplombTech.DWasa.Web.MqTTAdapter
{
    public class MqttClientWrapperAdapter
    {
        private static MqttClientWrapper instance = null;
        //Lock synchronization object
        private static object syncLock = new object();
        public static MqttClientWrapper WrapperInstance
        {
            get
            {
                lock (syncLock)
                {
                    if (MqttClientWrapperAdapter.instance == null)
                    {
                        instance = new MqttClientWrapper();
                        instance.NotifyMqttMsgPublishReceivedEvent += new MqttClientWrapper.NotifyMqttMsgPublishReceivedDelegate(PublishReceivedMessage_NotifyEvent);

                        instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_NotifyEvent);

                        instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_NotifyEvent);
                    }

                    return instance;
                }
            }
        }

        static void PublishReceivedMessage_NotifyEvent(CustomEventArgs customEventArgs)
        {
            if (customEventArgs.ReceivedTopic == CommandType.Configuration.ToString())
            {
                var jsonString = customEventArgs.ReceivedMessage;
                //LogJsonManager manager = new LogJsonManager(jsonString);
               // manager.Parse();
            }
        }

        static void PublishedMessage_NotifyEvent(CustomEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;

        }

        static void SubscribedMessage_NotifyEvent(CustomEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;
        }



    }
}
