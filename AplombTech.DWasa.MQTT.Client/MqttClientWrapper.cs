using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Json;
using AplombTech.DWasa.Model.Enums;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.DWasa.MQTT.Client
{
    public class MqttClientWrapper
    {

        static MqttClientWrapper()
        {
            ClientId = string.Empty;
        }

        public static void MakeConnection(string brokerAddress) // the global controlled variable
        {
            if (BDemoMQTT == null)
            {
                LocalBrokerConnection(brokerAddress);

                DefinedMQTTCommunicationEvents();
            }
        }



        public static bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            // logic for validation here
        }

        #region Properties

        public static string BrokerAddress
        {
            get;
        }
        public static MqttClient BDemoMQTT { get; set; }
        public static string ClientId { get; set; }
        public static string MQTT_BROKER_ADDRESS { get; set; }

        public static string ClientResponce { get; set; }
        #endregion

        #region Methods



        public static string Publish(string messgeTopic, string publishMessage)
        {

            ushort msgId = BDemoMQTT.Publish(messgeTopic, // topic
                                          Encoding.UTF8.GetBytes(publishMessage), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          true);
            return "Success";
        }

        public static string Subscribe(string messgeTopic)
        {
            ushort msgId = BDemoMQTT.Subscribe(new string[] { messgeTopic },
                new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
                );
            return "Success";
        }

        private static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            //e.IsPublished //it's defined confirmation message is published or not.
            // Debug.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
            ClientResponce = "Success";
        }



        public static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            //Debug.WriteLine("Subscribed for id = " + e.MessageId);
            // write your code
        }

        public static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            ClientResponce = "Success";
        }

        public static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var jsonString = Encoding.UTF8.GetString(e.Message);
            if (e.Topic == CommandType.Feedback.ToString())
            {
                LogJsonManager manager = new LogJsonManager(jsonString);
                manager.Parse();
            }

        }
        #endregion

        #region MQTT connection events

        private static void DefinedMQTTCommunicationEvents()
        {
            BDemoMQTT.MqttMsgPublished += client_MqttMsgPublished;//publish
            BDemoMQTT.MqttMsgSubscribed += client_MqttMsgSubscribed;//subscribe confirmation
            BDemoMQTT.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
            BDemoMQTT.MqttMsgPublishReceived += client_MqttMsgPublishReceived;//received message.

            //ushort submsgId = BDemoMQTT.Subscribe(new string[] { "/configuration", "/command", "/feedback" },
            //                  new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            //                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        private static void BrokerConnectionWithCertificate(string brokerAddress)
        {
            BDemoMQTT = new MqttClient(brokerAddress, MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
            BDemoMQTT.Connect(Guid.NewGuid().ToString(), "mosharraf", "mosharraf", false, 3600);
        }

        private static void BrokerConnectionWithoutCertificate(string brokerAddress)
        {
            BDemoMQTT = new MqttClient(brokerAddress, 18830, false, null, null, MqttSslProtocols.None, null);
            MQTTConnectiobn();
        }

        private static void LocalBrokerConnection(string brokerAddress)
        {
            BDemoMQTT = new MqttClient(brokerAddress);
            MQTTConnectiobn();
        }

        private static void MQTTConnectiobn()
        {
            BDemoMQTT.Connect(Guid.NewGuid().ToString());
        }
        #endregion
    }
}
