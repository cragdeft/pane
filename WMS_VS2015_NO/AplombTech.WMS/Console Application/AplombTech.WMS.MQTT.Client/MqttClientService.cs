using AplombTech.WMS.AreaRepositories;
using AplombTech.WMS.DataProcessRepository;
using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.JsonParser.Topics;
using AplombTech.WMS.JsonParser.Topics.Classification;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Utility;
using AplombTech.WMS.Utility.NakedObjects;
using NakedObjects;
using NakedObjects.Architecture.Component;
using NakedObjects.Async;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AplombTech.WMS.Domain.SummaryData;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using AplombTech.WMS.Persistence.UnitOfWorks;

namespace AplombTech.WMS.MQTT.Client
{
    public class MqttClientService: IWMSBatchStartPoint
    {
        #region Injected Services
        private INakedObjectsFramework _framework;
        private TransactionRunner _transactionRunner;
        private readonly ITopicClassifier _topicClassifier;
        private readonly IMessageParserFactory _messageParserFactory;
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        public IAsyncService AsyncService { private get; set; }
        #endregion

        public MqttClientService(ITopicClassifier topicClassifier, IMessageParserFactory messageParserFactory)
        {
            _topicClassifier = topicClassifier;
            _messageParserFactory = messageParserFactory;
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
        private void BrokerConnectionWithCertificate()
        {
            DhakaWasaMqtt = new MqttClient(GetBrokerAddress(), MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
            ConnectToBroker("kanok", "kanok");
        }

        public bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            // logic for validation here
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
        private ushort GetBrokerKeepAlivePeriod()
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
        private void ConnectToBroker(string username,string password)
        {
            DhakaWasaMqtt.Connect(GetClientId(), username, password, false, GetBrokerKeepAlivePeriod());
            log.Info("MQTT Client is connected via SSL");
        }
        private void DefinedMqttCommunicationEvents()
        {
            DhakaWasaMqtt.MqttMsgPublished += PublishedMessage_MQTT;//publish
            DhakaWasaMqtt.MqttMsgSubscribed += SubscribedMessage_MQTT;//subscribe confirmation
            DhakaWasaMqtt.MqttMsgUnsubscribed += UnsubscribedMessage_MQTT;
            DhakaWasaMqtt.MqttMsgPublishReceived += ReceivedMessage_MQTT;//received message.
            DhakaWasaMqtt.ConnectionClosed += ConnectionClosed_MQTT;

            ushort submsgId = DhakaWasaMqtt.Subscribe(new string[] { "wasa/configuration", "wasa/sensor_data" },
                              new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                      MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});

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
                        BrokerConnectionWithCertificate();
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
#if DEBUG
                
#else
            EmailSender.SendEmail("mosharraf.hossain@aplombtechbd.com;sumon.kumar@aplombtechbd.com", "mosharraf.hossain@aplombtechbd.com", "(Local Deploy)WMS:Could not stablished connection to MQTT broker", ex.Message);
#endif
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
                //throw new Exception("Could not stablished connection to MQTT broker");
                MakeConnection();
            }
        }
        private void ProcessMessage(string topic, string message)
        {            
            DataLog dataLog = LogSensorData(topic, message);

            if (dataLog != null)
            {
                if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
                {
                    try
                    {
                        var topicType = _topicClassifier.GetTopicType(topic);
                        var messageParser = _messageParserFactory.CreateMessageParser(topicType);
                        var parsedMessage = messageParser.ParseMessage(message);
                        _framework.TransactionManager.StartTransaction();
                        switch (topicType)
                        {
                            case TopicType.SensorData:
                                var deviceDataMessage = (SensorMessage)parsedMessage;
                                ProcessSensorData(deviceDataMessage);
                                ProcessMotorData(deviceDataMessage);                            
                                break;
                            case TopicType.Configuration:
                                var configDataMessage = (ConfigurationMessage)parsedMessage;
                                _transactionRunner.RunTransaction(
                                () => ProcessRepository.StoreConfigurationData(configDataMessage));
                                break;
                            default:
                                throw new InvalidTopicException();
                        }
                        UpdateDataLog(dataLog.SensorDataLogID, DataLog.ProcessingStatusEnum.Done,null);
                        _framework.TransactionManager.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                        _framework.TransactionManager.AbortTransaction();
                        UpdateDataLog(dataLog.SensorDataLogID, DataLog.ProcessingStatusEnum.Failed, "Error Occured in ProcessMessage method. Error: " + ex.ToString());
                    }
                }
            }
        }

        

        private void ProcessMotorData(SensorMessage messageObject)
        {
            foreach (MotorValue data in messageObject.Motors)
            {
                Motor motor = AreaRepository.FindMotorByUuid(data.MotorUid);
                if (motor != null && motor.IsActive)
                {
                    ProcessRepository.CreateNewMotorData(data, messageObject.LoggedAt, motor);
                    PublishMessageForMotorSummaryGeneration(data, messageObject.LoggedAt, motor);
                    if (motor is PumpMotor && data.MotorStatus == Motor.OFF)
                        PublishMotorAlertMessage(data, motor);
                }
            }
        }
        private void PublishMotorAlertMessage(MotorValue data, Motor motor)
        {
            var cmd = new MotorAlertMessage
            {
                MotorId = motor.MotorID,
                MotorName = GetMotorName(motor),             
                MotorStatus = data.MotorStatus,
                PumpStationName = motor.PumpStation.Name,
                AlertMessageType = (int)AlertType.AlertTypeEnum.OnOff,
                MessageDateTime = DateTime.Now
            };
            ServiceBus.Bus.Send(cmd);
        }
        private string GetMotorName(Motor motor)
        {
            string motorName = String.Empty;
            if (motor is PumpMotor)
            {
                motorName = "Pump Motor";
            }
            if (motor is ChlorineMotor)
            {
                motorName = "Chlorine Motor";
            }
            return motorName;
        }
        private void ProcessSensorData(SensorMessage messageObject)
        {
            foreach (SensorValue data in messageObject.Sensors)
            {
                Sensor sensor = AreaRepository.FindSensorByUuid(data.SensorUUID);
                if (sensor!=null && sensor.IsActive)
                {
                    ProcessRepository.CreateNewSensorData(data.Value, messageObject.LoggedAt, sensor);                    
                    PublishSensorMessageForSummaryGeneration(data, messageObject.LoggedAt, sensor);
                    PublishSensorAlertMessage(data.Value, sensor);
                }
            }
        }
        private void PublishSensorMessageForSummaryGeneration(SensorValue data, DateTime loggedAt, Sensor sensor)
        {
            var cmd = new SensorSummaryGenerationMessage
            {
                Uid = data.SensorUUID,
                Value = Convert.ToDecimal(data.Value),
                DataLoggedAt = loggedAt,
                MessageDateTime = DateTime.Now
            };
            if (sensor is FlowSensor || sensor is EnergySensor)
            {               
                if (cmd.Value >= 0)
                {
                    ServiceBus.Bus.Send(cmd);
                }               
            }
            else
            {
                ServiceBus.Bus.Send(cmd);
            }
        }
        private void PublishMessageForMotorSummaryGeneration(MotorValue data, DateTime loggedAt, Motor motor)
        {
            var cmd = new MotorSummaryGenerationMessage
            {
                Uid = data.MotorUid,
                MotorStatus = data.MotorStatus,
                DataLoggedAt = loggedAt,
                MessageDateTime = DateTime.Now
            };
            if (motor is PumpMotor)
            {
                ServiceBus.Bus.Send(cmd);
            }            
        }
        private void PublishSensorAlertMessage(string dataValue, Sensor sensor)
        {
            if (sensor is EnergySensor || sensor is ACPresenceDetector || sensor is BatteryVoltageDetector) return;

            string sensorName = GetSensorName(sensor);

            decimal value = 0;
            if (sensor.DataType == Sensor.Data_Type.Float)
                value = Convert.ToDecimal(dataValue);
            if (sensor.DataType == Sensor.Data_Type.Boolean)
            {
                if (dataValue != null && dataValue.Contains("."))
                {
                    value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(dataValue)));
                }
                else
                {
                    value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(dataValue)));
                }
            }
            
            if (value == 0)
            {
                if (sensor is ChlorinePresenceDetector)
                {
                    SendSensorAlertMessage(value, sensorName, (int)AlertType.AlertTypeEnum.OnOff, sensor);
                }
                else
                {
                    SendSensorAlertMessage(value, sensorName, (int)AlertType.AlertTypeEnum.DataMissing, sensor);
                }
                
                return;
            }

            if (!(sensor is ChlorinePresenceDetector))
            {
                //Get the minimum value
                decimal minimumValue = GetMinimumValue(sensor.SensorId);
                if (value < minimumValue)
                {
                    if (sensor.MinimumValue != minimumValue)
                    {
                        sensor.MinimumValue = minimumValue;
                    }
                    
                    SendSensorAlertMessage(value, sensorName, (int) AlertType.AlertTypeEnum.UnderThreshold, sensor);
                }
            }
        }

        private decimal GetMinimumValue(int sensorId)
        {
            decimal minimumValue = 0;
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                AplombTech.WMS.Persistence.Repositories.ProcessRepository repo = new AplombTech.WMS.Persistence.Repositories.ProcessRepository(uow.CurrentObjectContext);
                Sensor sensor = repo.GetSensorId(sensorId);
                minimumValue = sensor.MinimumValue;
            }

            return minimumValue;
        }
        private string GetSensorName(Sensor sensor)
        {
            string sensorName = String.Empty;
            if (sensor is FlowSensor)
            {
                sensorName = "Flow Sensor";
            }
            if (sensor is LevelSensor)
            {
                sensorName = "Level Sensor";
            }
            if (sensor is PressureSensor)
            {
                sensorName = "Pressure Sensor";
            }
            if (sensor is ChlorinePresenceDetector)
            {
                sensorName = "Chlorination Sensor";
            }

            if (sensor is ACPresenceDetector)
            {
                sensorName = "AC Presence Detector";
            }

            if (sensor is BatteryVoltageDetector)
            {
                sensorName = "Battery Voltage Detector";
            }
            return sensorName;
        }
        private void SendSensorAlertMessage(decimal value,  string sensorName, int allertMessageType, Sensor sensor)
        {
            if (allertMessageType == (int)AlertType.AlertTypeEnum.UnderThreshold)
            {
                ProcessRepository.CreateUnderThresoldData(value, sensor);
            }
            
            var cmd = new SensorAlertMessage
            {
                SensorId = sensor.SensorId,
                SensorName = sensorName,
                MinimumValue = sensor.MinimumValue,
                Value = value,
                PumpStationName = sensor.PumpStation.Name,
                AlertMessageType = allertMessageType,
                MessageDateTime = DateTime.Now
            };
            ServiceBus.Bus.Send(cmd);
        }

        

        private DataLog LogSensorData(string topic, string message)
        {
            try
            {
                _framework.TransactionManager.StartTransaction();
                DataLog dataLog = ProcessRepository.LogData(topic, message);
                _framework.TransactionManager.EndTransaction();

                return dataLog;
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                _framework.TransactionManager.AbortTransaction();
                return null;
            }
        }

        private void UpdateDataLog(long sensorDataLogId, DataLog.ProcessingStatusEnum status,string remarks)
        {
            ProcessRepository.UpdateDataLog(sensorDataLogId,status,remarks);
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
            this._framework = objframework;
            _transactionRunner = new TransactionRunner(_framework.TransactionManager);
            log.Info("MQTT listener is going to start");
            ServiceBus.Init();
#if DEBUG
            MqttClientInstance(true);
#else
            MqttClientInstance(true);
#endif
            log.Info("MQTT listener has been started");
        }       
    }
}
