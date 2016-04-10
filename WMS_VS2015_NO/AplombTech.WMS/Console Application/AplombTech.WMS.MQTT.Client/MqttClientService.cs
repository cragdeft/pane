using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using NakedObjects.Architecture.Component;
using NakedObjects.Async;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.WMS.MQTT.Client
{
    public class MqttClientService: IMyBatchStartPoint
    {
        #region Injected Services
        private INakedObjectsFramework framework;
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        public IAsyncService AsyncService { private get; set; }
        #endregion

        public MqttClientService()
        {
            
        }
        
        public enum JsonMessageType
        {
            configuration,
            sensordata,
            feedback
        }
        private MqttClient DhakaWasaMQTT { get; set; }
        private bool IsSSL { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void MQTTClientInstance(bool isSSL)
        {
            IsSSL = isSSL;
            MakeConnection();
        }
        private void BrokerConnectionWithoutCertificate()
        {
            DhakaWasaMQTT = new MqttClient(GetBrokerAddress(), GetBrokerPort(), false, null, null, MqttSslProtocols.None, null);
            ConnectToBroker();
        }
        private string GetBrokerAddress()
        {
            if (ConfigurationManager.AppSettings["BrokerAddress"] == null)
            {
                return string.Empty;
            }
            return ConfigurationManager.AppSettings["BrokerAddress"].ToString();
        }
        private int GetBrokerPort()
        {
            if (ConfigurationManager.AppSettings["BrokerPort"] == null)
            {
                return 1883;
            }
            return Convert.ToInt32(ConfigurationManager.AppSettings["BrokerPort"]);
        }
        private UInt16 GetBrokerKeepAlivePeriod()
        {
            if (ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"] == null)
            {
                return 3600;
            }
            return Convert.ToUInt16(ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"]);
        }
        private string GetClientId()
        {
            if (ConfigurationManager.AppSettings["BrokerAccessClientId"] == null)
            {
                string clientId = Guid.NewGuid().ToString();
                return clientId;
            }
            return ConfigurationManager.AppSettings["BrokerAccessClientId"].ToString();
        }
        private void ConnectToBroker()
        {
            DhakaWasaMQTT.Connect(GetClientId(), null, null, false, GetBrokerKeepAlivePeriod());
            log.Info("MQTT Client is connected");
        }
        private void DefinedMQTTCommunicationEvents()
        {
            DhakaWasaMQTT.MqttMsgPublished += PublishedMessage_MQTT;//publish
            DhakaWasaMQTT.MqttMsgSubscribed += SubscribedMessage_MQTT;//subscribe confirmation
            DhakaWasaMQTT.MqttMsgUnsubscribed += UnsubscribedMessage_MQTT;
            DhakaWasaMQTT.MqttMsgPublishReceived += ReceivedMessage_MQTT;//received message.
            DhakaWasaMQTT.ConnectionClosed += ConnectionClosed_MQTT;

            ushort submsgId = DhakaWasaMQTT.Subscribe(new string[] { "/configuration", "/command", "/feedback", "/sensordata" },
                              new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                      MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }
        private void HandleReconnect()
        {
            MakeConnection();
        }
        private void MakeConnection()
        {
            try
            {
                if (DhakaWasaMQTT == null || !DhakaWasaMQTT.IsConnected)
                {
                    if (IsSSL)
                    {
                        //BrokerConnectionWithCertificate();
                    }
                    else
                    {
                        BrokerConnectionWithoutCertificate();
                    }
                    DefinedMQTTCommunicationEvents();
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not stablished connection to MQTT broker - " + ex.Message);

                //don't leave the client connected
                if (DhakaWasaMQTT != null && DhakaWasaMQTT.IsConnected)
                {
                    try
                    {
                        DhakaWasaMQTT.Disconnect();
                    }
                    catch
                    {
                        log.Error(string.Format("Could not disconnect to MQTT broker: {1}", ex.Message));
                    }
                }
                throw new Exception("Could not stablished connection to MQTT broker");
            }
        }
        private void ProcessMessage(string topic, string message)
        {
            SensorDataLog dataLog = LogSensorData(topic, message);
            try
            {
                if (dataLog != null)
                {
                    if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
                    {
                        framework.TransactionManager.StartTransaction();
                        if (topic.Replace("/", String.Empty) == JsonMessageType.sensordata.ToString())
                        {
                            ProcessRepository.ParseNStoreSensorData(dataLog);
                        }
                        if (topic.Replace("/", String.Empty) == JsonMessageType.configuration.ToString())
                        {
                            ProcessRepository.ParseNStoreConfigurationData(dataLog);
                        }
                        framework.TransactionManager.EndTransaction();
                    }
                }               
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                dataLog.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.Failed;
                framework.TransactionManager.AbortTransaction();
            }            
        }
        private SensorDataLog LogSensorData(string topic, string message)
        {
            try
            {
                framework.TransactionManager.StartTransaction();
                SensorDataLog dataLog = ProcessRepository.LogSensorData(topic, message);
                framework.TransactionManager.EndTransaction();

                if (dataLog == null)
                {
                    Publish(topic + JsonMessageType.feedback.ToString(), "Logged Date & Time is missing");
                    return null;
                }
                Publish(topic + JsonMessageType.feedback.ToString(), "Message has been logged Sucessfully");
                return dataLog;
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                framework.TransactionManager.AbortTransaction();
                return null;
            }
        }
        private string Publish(string messgeTopic, string publishMessage)
        {
            if (DhakaWasaMQTT != null)
            {
                try
                {
                        ushort msgId = DhakaWasaMQTT.Publish(messgeTopic, // topic
                                          Encoding.UTF8.GetBytes(publishMessage), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          true);
                }
                catch (Exception ex)
                {
                    //    log.Warn("Error while publishing: " + ex.Message, ex);
                }
            }
            return "Success";
        }

        #region EVENT HANDLER
        private void PublishedMessage_MQTT(object sender, MqttMsgPublishedEventArgs e)
        {
            //NotifyMessage("MqttMsgPublished", e.IsPublished.ToString(), string.Empty);
            log.Info(string.Format("Mqtt-Msg-Published to topic {0}", e.IsPublished.ToString()));
            //ClientResponce = "Success";
        }
        private void SubscribedMessage_MQTT(object sender, MqttMsgSubscribedEventArgs e)
        {
            //NotifyMessage("MqttMsgSubscribed", e.MessageId.ToString(), string.Empty);
            log.Info(string.Format("Mqtt-Msg-Subscribed to topic {0}", e.MessageId.ToString()));
        }
        private void UnsubscribedMessage_MQTT(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            //ClientResponce = "Success";
        }
        private void ReceivedMessage_MQTT(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);
            string topic = e.Topic.ToString();
            log.Info("Message received from topic '" + topic + "' and message is '" + message + "'");
            ProcessMessage(topic,message);
            //AsyncService.RunAsync((domainObjectContainer) =>
            //                 ProcessMessage(topic, message));
        }
        private void ConnectionClosed_MQTT(object sender, EventArgs e)
        {
            if (!(sender as MqttClient).IsConnected || DhakaWasaMQTT == null)
            {
                HandleReconnect();
            }
            log.Info("Connection has been closed");
        }
        #endregion
        public void Execute(INakedObjectsFramework framework)
        {
            this.framework = framework;
            log.Info("MQTT listener is going to start");
            MQTTClientInstance(false);
            log.Info("MQTT listener has been started");
        }
        
    }
}
