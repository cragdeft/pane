using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
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
        public IAsyncService AsyncService { private get; set; }
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
            //var data = "{"name":"masnun","email":["masnun@gmail.com","masnun@leevio.com"],"websites":{"home page":"http:\/\/masnun.com","blog":"http:\/\/masnun.me"}}"
            //JObject o = JObject.Parse(data);
            //Console.WriteLine("Name: " + o["name"]);
            //Console.WriteLine("Email Address[1]: " + o["email"][0]);
            //Console.WriteLine("Email Address[2]: " + o["email"][1]);
            //Console.WriteLine("Website [home page]: " + o["websites"]["home page"]);
            //Console.WriteLine("Website [blog]: " + o["websites"]["blog"]);

            //AsyncService.RunAsync((domainObjectContainer) =>
            //             MqttClientFacade.MQTTClientInstance(false));

            IList<Zone> zones = AreaRepository.AllZones().ToList();
//#if DEBUG
//            Debug.WriteLine(customEventArgs.ReceivedTopic);
//#endif
            instance.Publish("/topic", "Message Received with Thanks");
            if (customEventArgs.ReceivedTopic == "/configuration")
            {
                string msg = customEventArgs.ReceivedMessage;
                int count = zones.Count();
//#if DEBUG
//                Debug.WriteLine(msg);
//                Debug.WriteLine(count);
//#endif
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
