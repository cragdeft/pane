using AplombTech.WMS.JsonParser.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Devices;

namespace AplombTech.WMS.JsonParser
{
    public class JsonManager
    {
        public static DateTime GetSensorLoggedAtTime(string message)
        {
            JObject o = JObject.Parse(message);

            string loggedAt = o["PumpStation"]["LogDateTime"].ToString();

            DateTime loggesAtTime = Convert.ToDateTime(loggedAt);
            return loggesAtTime;
        }
        public static bool GetPumpStationSensorDataComplete(string message)
        {
            JObject o = JObject.Parse(message);

            string sensorDataComplete = o["PumpStation"]["SensorDataComplete"].ToString();

            bool dataComplete = Convert.ToBoolean(sensorDataComplete);
            return dataComplete;
        }
        public static int GetSensorDataLogCount(string message)
        {
            JObject o = JObject.Parse(message);

            string logCount = o["PumpStation"]["logCnt"].ToString();

            int count = Convert.ToInt32(logCount);
            return count;
        }
        public static DateTime GetConfigurationLoggedAtTime(string message)
        {
            JObject o = JObject.Parse(message);

            string loggedAt = o["PumpStation"]["ConfigureDateTime"].ToString();
            DateTime loggesAtTime = Convert.ToDateTime(loggedAt);
            return loggesAtTime;
        }
        public static int GetPumpStationIDFromJson(string message)
        {
            JObject o = JObject.Parse(message);

            string pumpStationId = o["PumpStation"]["PumpStation_Id"].ToString();
            int stationId = Convert.ToInt32(pumpStationId);
            return stationId;
        }
        
        public static SensorMessage GetSensorObject(string message)
        {
            SensorMessage sensorObject = new SensorMessage();
            try
            {
                JObject o = JObject.Parse(message);
                //o["Sensor"][0]["uid"],o["Sensor"][0]["value"]
                sensorObject.PumpStationId = GetPumpStationIDFromJson(message);
                sensorObject.LoggedAt = GetSensorLoggedAtTime(message);
                //for (int i = 0; i < o["PumpStation"]["Sensors"]["PT"].Count(); i++)
                //{
                //    configurationObject.Sensors.Add(GetSensor(o, i, "PT"));
                //}
                //if (!string.IsNullOrEmpty((o["PumpStation"]["Motor"]["Pump_Motor"]).ToString()))
                //{
                //    configurationObject.PumpMotor = GetPumpMotor(o);
                //}
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetAcpSensorData("ACP", o)); ;
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("BV", o, 0)); ;
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("CPD", o, 0)); ;
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("ET", o, 0)); ;
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("FT", o, 0));
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("LT", o, 0)); ;
                }
                if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
                {
                    sensorObject.Sensors.Add(GetSensorData("PT", o, 0));
                }
                if (!string.IsNullOrEmpty((o["Motors"]).ToString()))
                {
                    sensorObject.Motors.Add(GetMotorData("Pump_Motor", o, 0));
                }

                if (!string.IsNullOrEmpty((o["Motors"]).ToString()))
                {
                    sensorObject.Motors.Add(GetMotorData("Chlorine_Motor", o, 0));
                }
                return sensorObject;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static ConfigurationMessage GetConfigurationObject(string message)
        {
            JObject o = JObject.Parse(message);

            ConfigurationMessage configurationObject = new ConfigurationMessage();

            configurationObject.PumpStationId = GetPumpStationIDFromJson(message);
            configurationObject.LoggedAt = GetConfigurationLoggedAtTime(message);

            //for (int i = 0; i < o["PumpStation"]["Camera"].Count(); i++)
            //{
            //    configurationObject.Cameras.Add(GetCamera(o, i));
            //}

            if (!string.IsNullOrEmpty((o["PumpStation"]["Sensors"]["ACP"]).ToString()))
            {
                configurationObject.Sensors.Add(GetAcpSensor(o, "ACP"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["BV"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i, "BV"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["CPD"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i,"CPD"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["ET"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i,"ET"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["FT"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i,"FT"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["LT"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i,"LT"));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["PT"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i,"PT"));
            }
            if (!string.IsNullOrEmpty((o["PumpStation"]["Motor"]["Pump_Motor"]).ToString()))
            {
                configurationObject.PumpMotor = GetPumpMotor(o);
            }
            if (!string.IsNullOrEmpty((o["PumpStation"]["Motor"]["Pump_Motor"]).ToString()))
            {
                configurationObject.ChlorineMotor = GetCholorineMotor(o);
            }
            //configurationObject.Router = (GetRouter(o));
            
            

            return configurationObject;
        }

        private static SensorValue GetSensorData(string root,JObject o, int index)
        {
            SensorValue data = new SensorValue();
            data.SensorUUID = (string)o["Sensors"][root][index]["uid"];
            data.Value = (string)o["Sensors"][root][index]["value"];


            return data;
        }

        private static SensorValue GetAcpSensorData(string root, JObject o)
        {
            SensorValue data = new SensorValue();
            data.SensorUUID = (string)o["Sensors"][root]["uid"];
            data.Value = (string)o["Sensors"][root]["value"];


            return data;
        }

        private static MotorValue GetMotorData(string root, JObject o, int index)
        {
            MotorValue data = new MotorValue();
            data.MotorUid = (string)o["Motors"][root][index]["uid"];
            data.Auto = string.IsNullOrEmpty((string)o["Motors"][root][index]["Auto"])? false:(bool)o["Motors"][root][index]["Auto"];
            data.MotorStatus = (string)o["Motors"][root][index]["Motor_Status"];
            data.LastCommand = (string)o["Motors"][root][index]["Last_Command"];
            data.LastCommandTime = (string)o["Motors"][root][index]["Last_Command_Time"];

            return data;
        }
        private static Camera GetCamera(JObject o, int index)
        {
            Camera camera = new Camera();
            camera.UUID = (string)o["PumpStation"]["Camera"][index]["uid"];
            camera.URL = (string)o["PumpStation"]["Camera"][index]["url"];

            return camera;
        }
        private static Router GetRouter(JObject o)
        {
            Router router = new Router();
            router.UUID = (string)o["PumpStation"]["Router"]["mac_id"];
            router.MACAddress = (string)o["PumpStation"]["Router"]["mac_id"];
            router.IP = (string)o["PumpStation"]["Router"]["ip"];
            router.Port = (int)o["PumpStation"]["Router"]["port"];

            return router;
        }

        private static PumpMotor GetPumpMotor(JObject o)
        {
            PumpMotor pump = new PumpMotor();
            pump.UUID = (string)o["PumpStation"]["Motor"]["Pump_Motor"][0]["uid"];
            pump.Controllable = (bool)o["PumpStation"]["Motor"]["Pump_Motor"][0]["Controllable"];
            pump.Auto = (bool)o["PumpStation"]["Motor"]["Pump_Motor"][0]["Auto"];
            return pump;
        }

        private static ChlorineMotor GetCholorineMotor(JObject o)
        {
            ChlorineMotor motor = new ChlorineMotor();
            motor.UUID = (string)o["PumpStation"]["Motor"]["Chlorine_Motor"][0]["uid"];
            motor.Controllable = (bool)o["PumpStation"]["Motor"]["Chlorine_Motor"][0]["Controllable"];
            motor.Auto = (bool)o["PumpStation"]["Motor"]["Chlorine_Motor"][0]["Auto"];
            return motor;
        }
        private static Sensor GetSensor(JObject o, int index, string rootname)
        {
            string type = rootname;
            string name = (string)o["PumpStation"]["Sensors"][rootname]["name"];
            string uid = (string)o["PumpStation"]["Sensors"][rootname][index]["uid"];
            string dataType = (string)o["PumpStation"]["Sensors"][rootname][index]["Data_Type"];
            string unit = (string) o["PumpStation"]["Sensors"][rootname][index]["Unit"];
            string model = (string) o["PumpStation"]["Sensors"][rootname][index]["Model"];
            string version = (string) o["PumpStation"]["Sensors"][rootname][index]["Version"];
            //Sensor sensor = GetCommonSensor(uid);
            return GetMatchedSensor(type, uid,dataType,unit,model,version,name);
        }

        private static Sensor GetAcpSensor(JObject o, string rootname)
        {
            string type = rootname;
            string name = (string) o["PumpStation"]["Sensors"][rootname]["name"];
            string uid = (string)o["PumpStation"]["Sensors"][rootname]["uid"];
            string dataType = (string)o["PumpStation"]["Sensors"][rootname]["Data_Type"];
            string unit = (string)o["PumpStation"]["Sensors"][rootname]["Unit"];
            string model = (string)o["PumpStation"]["Sensors"][rootname]["Model"];
            string version = (string)o["PumpStation"]["Sensors"][rootname]["Version"];
            //Sensor sensor = GetCommonSensor(uid);
            return GetMatchedSensor(type, uid, dataType, unit, model, version,name);
        }
        private static Sensor GetMatchedSensor(string type, string uid,string dataType,string unit,string model,string version,string name)
        {
            switch (type)
            {
                case "FT":
                    {
                        return CreateFlowSensor(uid, dataType,unit,model,version,name);
                    }
                case "ET":
                    {
                        return CreateEnergySensor(uid, dataType, unit, model, version, name);
                    }
                case "LT":
                    {
                        return CreateLevelSensor(uid, dataType, unit, model, version, name);
                    }
                case "PT":
                    {
                        return CreatePressureSensor(uid, dataType, unit, model, version, name);
                    }
                case "CPD":
                    {
                        return CreateChlorinationSensor(uid, dataType, unit, model, version, name);
                    }
                case "ACP":
                    {
                        return CreateAcPresenseDector(uid, dataType, unit, model, version, name);
                    }
                case "BV":
                    {
                        return CreateBatteryVoltageDetector(uid, dataType, unit, model, version, name);
                    }
            }

            return null;
        }
        private static Sensor CreateChlorinationSensor(string uid, string dataType, string unit, string model, string version,string name)
        {
            ChlorinePresenceDetector sensor = new ChlorinePresenceDetector();
            sensor.Name = name;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreatePressureSensor(string uid, string dataType, string unit, string model, string version, string name)
        {
            PressureSensor sensor = new PressureSensor();
            sensor.Name = name;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreateLevelSensor(string uid, string dataType, string unit, string model, string version, string name)
        {
            LevelSensor sensor = new LevelSensor();
            sensor.Name = name;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreateEnergySensor(string uid, string dataType, string unit, string model, string version, string name)
        {
            EnergySensor sensor = new EnergySensor();
            sensor.Name = name;
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreateFlowSensor(string uid, string dataType, string unit, string model, string version, string name)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.Name = name;
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreateAcPresenseDector(string uid, string dataType, string unit, string model, string version, string name)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.Name = name;
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor CreateBatteryVoltageDetector(string uid, string dataType, string unit, string model, string version, string name)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.Name = name;
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.DataType = (Sensor.Data_Type)Enum.Parse(typeof(Sensor.Data_Type), dataType, true);
            sensor.UnitName = unit;
            sensor.Model = model;
            sensor.Version = version;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;
            return sensor;
        }
        private static Sensor GetCommonSensor(string uid)
        {
            Sensor sensor = new Sensor();
            sensor.MaximumValue = 0;
            sensor.CurrentValue = 0;
            sensor.MinimumValue = 0;
            sensor.UUID = uid;
            sensor.AuditFields.InsertedBy = "Automated";
            sensor.AuditFields.InsertedDateTime = DateTime.Now;
            sensor.AuditFields.LastUpdatedBy = "Automated";
            sensor.AuditFields.LastUpdatedDateTime = DateTime.Now;

            return sensor;
        }
    }
}
