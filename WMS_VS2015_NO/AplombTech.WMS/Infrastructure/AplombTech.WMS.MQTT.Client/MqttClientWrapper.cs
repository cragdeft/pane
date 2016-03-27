﻿using AplombTech.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.MQTT.Client
{
    public class MqttClientWrapper
    {
        #region delegate event
        #region MqttMsg-Publish-Received-Notification
        public delegate void NotifyMqttMessageReceivedDelegate(MQTTEventArgs customEventArgs);
        public event NotifyMqttMessageReceivedDelegate NotifyMqttMessageReceivedEvent;
        #endregion

        #region MqttMsg-Published-Notification
        public delegate void NotifyMqttMsgPublishedDelegate(MQTTEventArgs customEventArgs);
        public event NotifyMqttMsgPublishedDelegate NotifyMqttMsgPublishedEvent;
        #endregion

        #region MqttMsg-Subscribed-Notification
        public delegate void NotifyMqttMsgSubscribedDelegate(MQTTEventArgs customEventArgs);
        public event NotifyMqttMsgSubscribedDelegate NotifyMqttMsgSubscribedEvent;
        #endregion

        #endregion

        #region constructor
        public MqttClientWrapper()
        {
        }
        public void MakeConnection()
        {
            #region MyRegion

            try
            {
                if (SmartHomeMQTT == null || !SmartHomeMQTT.IsConnected)
                {
                    if (BrokerAddress == "192.168.11.195")
                    {
                        LocalBrokerConnection(BrokerAddress);
                    }
                    else if (BrokerAddress == "192.168.11.150")
                    {
                        BrokerConnectionWithoutCertificate(BrokerAddress);
                    }
                    else
                    {
                        BrokerConnectionWithCertificate(BrokerAddress);
                    }
                    DefinedMQTTCommunicationEvents();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, string.Format("Could not stablished connection to MQ broker: {1}", ex.Message));

                //don't leave the client connected
                if (SmartHomeMQTT != null && SmartHomeMQTT.IsConnected)
                    try
                    {
                        SmartHomeMQTT.Disconnect();
                    }
                    catch
                    {
                        Logger.LogError(ex, string.Format("Could not disconnect to MQ broker: {1}", ex.Message));
                    }
            }
            #endregion
        }
        public bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            // logic for validation here
        }
        #endregion

        #region Properties
        public string WillTopic { get; set; }
        readonly object locker = new object();
        public string BrokerAddress
        {
            get
            {
                if (ConfigurationManager.AppSettings["BrokerAddress"] == null)
                {
                    return string.Empty;
                }
                return ConfigurationManager.AppSettings["BrokerAddress"].ToString();
            }
        }
        public int BrokerPort
        {
            get
            {
                if (ConfigurationManager.AppSettings["BrokerPort"] == null)
                {
                    return 1883;
                }
                return Convert.ToInt32(ConfigurationManager.AppSettings["BrokerPort"]);
            }
        }
        public UInt16 BrokerKeepAlivePeriod
        {
            get
            {
                if (ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"] == null)
                {
                    return 3600;
                }
                return Convert.ToUInt16(ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"]);
            }
        }
        public string ClientId
        {
            get
            {
                if (ConfigurationManager.AppSettings["BrokerAccessClientId"] == null)
                {
                    string clientId = Guid.NewGuid().ToString();
                    return clientId;
                }
                return ConfigurationManager.AppSettings["BrokerAccessClientId"].ToString();
            }
        }
        public MqttClient SmartHomeMQTT { get; set; }
        public string ClientResponce { get; set; }
        //public MqttClientWrapper.NotifyMqttMessageReceivedDelegate NotifyMqttMsgPublishReceivedEvent { get; set; }
        #endregion

        #region Methods
        public string Publish(string messgeTopic, string publishMessage)
        {
            if (SmartHomeMQTT != null)
            {
                try
                {
                    lock (locker)
                    {
                        ushort msgId = SmartHomeMQTT.Publish(messgeTopic, // topic
                                          Encoding.UTF8.GetBytes(publishMessage), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          true);
                    }
                }
                catch (Exception ex)
                {
                    //    log.Warn("Error while publishing: " + ex.Message, ex);
                }
            }
            return "Success";
        }

        public string Subscribe(string messgeTopic)
        {
            if (SmartHomeMQTT != null)
            {
                ushort msgId = SmartHomeMQTT.Subscribe(new string[] { messgeTopic },
                     new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
                     );
                Logger.Log(string.Format("Subscription to topic {0}", messgeTopic));
            }
            return "Success";
        }

        public void Subscribe(IEnumerable<string> messgeTopics)
        {
            foreach (var item in messgeTopics)
                Subscribe(item);
        }

        private void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            NotifyMessage("MqttMsgPublished", e.IsPublished.ToString(), string.Empty);
            Logger.Log(string.Format("Mqtt-Msg-Published to topic {0}", e.IsPublished.ToString()));
            ClientResponce = "Success";
        }

        public void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            NotifyMessage("MqttMsgSubscribed", e.MessageId.ToString(), string.Empty);
            Logger.Log(string.Format("Mqtt-Msg-Subscribed to topic {0}", e.MessageId.ToString()));
        }

        public void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            ClientResponce = "Success";
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            NotifyMessage("MqttMsgPublishReceived", Encoding.UTF8.GetString(e.Message), e.Topic.ToString());
            Logger.Log(string.Format("Mqtt-Msg-Publish-Received to topic {0}", e.Topic.ToString()));
        }

        public void client_ConnectionClosed(object sender, EventArgs e)
        {
            if (!(sender as MqttClient).IsConnected || SmartHomeMQTT == null)
            {
                HandleReconnect();
            }
            Logger.Log("Connection has been closed");
        }

        void HandleReconnect()
        {
            MakeConnection();
        }

        #region Delegate and event implementation
        public void NotifyMessage(string NotifyType, string receivedMessage, string receivedTopic)
        {
            if (NotifyType == "MqttMsgPublishReceived")
            {
                InvokeEvents<NotifyMqttMessageReceivedDelegate>(receivedMessage, receivedTopic, NotifyMqttMessageReceivedEvent);
            }

            if (NotifyType == "MqttMsgPublished")
            {
                InvokeEvents<NotifyMqttMsgPublishedDelegate>(receivedMessage, receivedTopic, NotifyMqttMsgPublishedEvent);
            }

            if (NotifyType == "MqttMsgSubscribed")
            {
                InvokeEvents<NotifyMqttMsgSubscribedDelegate>(receivedMessage, receivedTopic, NotifyMqttMsgSubscribedEvent);
            }
        }

        private static void InvokeEvents<T>(string receivedMessage, string receivedTopic, T eventDelegate)
        {
            if (eventDelegate != null)
            {
                var customEventArgs = new MQTTEventArgs(receivedMessage, receivedTopic);
                ((Delegate)(object)eventDelegate).DynamicInvoke(customEventArgs);
            }
        }
        #endregion

        #endregion

        #region MQTT connection events

        private void DefinedMQTTCommunicationEvents()
        {
            SmartHomeMQTT.MqttMsgPublished += client_MqttMsgPublished;//publish
            SmartHomeMQTT.MqttMsgSubscribed += client_MqttMsgSubscribed;//subscribe confirmation
            SmartHomeMQTT.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
            SmartHomeMQTT.MqttMsgPublishReceived += client_MqttMsgPublishReceived;//received message.
            SmartHomeMQTT.ConnectionClosed += client_ConnectionClosed;

            ushort submsgId = SmartHomeMQTT.Subscribe(new string[] { "/configuration", "/command", "/feedback" },
                              new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                      MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        private void BrokerConnectionWithCertificate(string brokerAddress)
        {
            //SmartHomeMQTT = new MqttClient(brokerAddress, MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
            SmartHomeMQTT.Connect(ClientId, "mosharraf", "mosharraf", false, BrokerKeepAlivePeriod);
        }

        private void BrokerConnectionWithoutCertificate(string brokerAddress)
        {
            SmartHomeMQTT = new MqttClient(brokerAddress, BrokerPort, false, null, null, MqttSslProtocols.None, null);
            MQTTConnectiobn();
        }

        private void LocalBrokerConnection(string brokerAddress)
        {
            SmartHomeMQTT = new MqttClient(brokerAddress);
            MQTTConnectiobn();
        }

        private void MQTTConnectiobn()
        {
            SmartHomeMQTT.Connect(ClientId, null, null, false, BrokerKeepAlivePeriod);
        }
        #endregion
    }


    public class MQTTEventArgs : EventArgs
    {
        public MQTTEventArgs(string receivedMessage, string receivedTopic)
        {
            _receivedMessage = receivedMessage;
            _receivedTopic = receivedTopic;
        }
        private string _receivedMessage;

        public string ReceivedMessage
        {
            get { return _receivedMessage; }
            set { _receivedMessage = value; }
        }

        private string _receivedTopic;
        public string ReceivedTopic
        {
            get { return _receivedTopic; }
            set { _receivedTopic = value; }
        }
    }
}
