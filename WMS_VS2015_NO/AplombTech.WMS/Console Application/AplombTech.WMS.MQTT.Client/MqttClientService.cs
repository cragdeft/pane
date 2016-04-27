using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Messages.Commands;
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
    public class MqttClientService: IWMSBatchStartPoint
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
        private MqttClient DhakaWasaMqtt { get; set; }
        private bool IsSsl { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void MqttClientInstance(bool isSSL)
        {
            IsSsl = isSSL;
            MakeConnection();
        }
        private void BrokerConnectionWithoutCertificate()
        {
            DhakaWasaMqtt = new MqttClient(GetBrokerAddress(), GetBrokerPort(), false, null, null, MqttSslProtocols.None, null);
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
            DhakaWasaMqtt.Connect(GetClientId(), null, null, false, GetBrokerKeepAlivePeriod());
            log.Info("MQTT Client is connected");
        }
        private void DefinedMqttCommunicationEvents()
        {
            DhakaWasaMqtt.MqttMsgPublished += PublishedMessage_MQTT;//publish
            DhakaWasaMqtt.MqttMsgSubscribed += SubscribedMessage_MQTT;//subscribe confirmation
            DhakaWasaMqtt.MqttMsgUnsubscribed += UnsubscribedMessage_MQTT;
            DhakaWasaMqtt.MqttMsgPublishReceived += ReceivedMessage_MQTT;//received message.
            DhakaWasaMqtt.ConnectionClosed += ConnectionClosed_MQTT;

            ushort submsgId = DhakaWasaMqtt.Subscribe(new string[] { "/configuration", "/command", "/feedback", "/sensordata" },
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
                if (DhakaWasaMqtt == null || !DhakaWasaMqtt.IsConnected)
                {
                    if (IsSsl)
                    {
                        //BrokerConnectionWithCertificate();
                    }
                    else
                    {
                        BrokerConnectionWithoutCertificate();
                    }
                    DefinedMqttCommunicationEvents();
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not stablished connection to MQTT broker - " + ex.Message);

                //don't leave the client connected
                if (DhakaWasaMqtt != null && DhakaWasaMqtt.IsConnected)
                {
                    try
                    {
                        DhakaWasaMqtt.Disconnect();
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
            DataLog dataLog = LogSensorData(topic, message);

            if (dataLog != null)
            {
                if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
                {
                    PublishMessage(dataLog);
                    try
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
                    catch (Exception ex)
                    {
                        log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                        framework.TransactionManager.AbortTransaction();
                        framework.TransactionManager.StartTransaction();
                        dataLog.ProcessingStatus = DataLog.ProcessingStatusEnum.Failed;
                        dataLog.Remarks = "Error Occured in ProcessMessage method. Error: " + ex.ToString();
                        framework.TransactionManager.EndTransaction();
                    }
                }
            }
        }
        private DataLog LogSensorData(string topic, string message)
        {
            try
            {
                framework.TransactionManager.StartTransaction();
                DataLog dataLog = ProcessRepository.LogData(topic, message);
                framework.TransactionManager.EndTransaction();

                //if (dataLog == null)
                //{
                //    Publish(topic + JsonMessageType.feedback.ToString(), "Logged Date & Time is missing");
                //    return null;
                //}
                //Publish(topic + JsonMessageType.feedback.ToString(), "Message has been logged Sucessfully");
                return dataLog;
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                framework.TransactionManager.AbortTransaction();
                return null;
            }
        }

        private void PublishMessage(DataLog datalog)
        {
            var cmd = new ProcessSensorData
            {
                SensorDataLogId = datalog.SensorDataLogID,
                Topic = datalog.Topic,
                Message = datalog.Message,
                LoggedAtSensor = datalog.LoggedAtSensor
            };

            ServiceBus.Bus.Send(cmd);
        }
        private string Publish(string messgeTopic, string publishMessage)
        {
            if (DhakaWasaMqtt != null)
            {
                try
                {
                        ushort msgId = DhakaWasaMqtt.Publish(messgeTopic, // topic
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
            if (!(sender as MqttClient).IsConnected || DhakaWasaMqtt == null)
            {
                HandleReconnect();
            }
            log.Info("Connection has been closed");
        }
        #endregion
        public void Execute(INakedObjectsFramework objframework)
        {
            this.framework = objframework;
            log.Info("MQTT listener is going to start");
            ServiceBus.Init();
            MqttClientInstance(false);
            log.Info("MQTT listener has been started");
        }
        
    }
}
