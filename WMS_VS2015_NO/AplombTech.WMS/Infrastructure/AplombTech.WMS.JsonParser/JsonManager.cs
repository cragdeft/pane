using AplombTech.WMS.JsonParser.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Motors;
using AplombTech.WMS.QueryModel.Sensors;

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

            string pumpStationId = o["PumpStation"]["PumoStation_Id"].ToString();
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
                sensorObject.SensorLoggedAt = GetSensorLoggedAtTime(message);

                for (int i = 0; i < 5; i++)
                {
                    sensorObject.Sensors.Add(GetSensorData(o, i));
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
            configurationObject.ConfigurationLoggedAt = GetConfigurationLoggedAtTime(message);

            for (int i = 0; i < o["PumpStation"]["Camera"].Count(); i++)
            {
                configurationObject.Cameras.Add(GetCamera(o, i));
            }

            for (int i = 0; i < o["PumpStation"]["Sensors"]["ACP"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i, "ACP"));
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

            //configurationObject.Router = (GetRouter(o));
            configurationObject.PumpMotor = GetPumpMotor(o);
            configurationObject.ChlorineMotor = GetCholorineMotor(o);

            return configurationObject;
        }

        private static SensorValue GetSensorData(JObject o, int index)
        {
            SensorValue data = new SensorValue();
            data.SensorUUID = (string)o["PumpStation"]["Sensor"][index]["uid"];
            data.Value = (string)o["PumpStation"]["Sensor"][index]["value"];


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
            pump.UUID = (string)o["PumpStation"]["Motor"]["Pump_Motor"]["uid"];
            pump.Controllable = (bool)o["PumpStation"]["Motor"]["Pump_Motor"]["Controllable"];
            pump.Auto = (bool)o["PumpStation"]["Motor"]["Pump_Motor"]["Auto"];
            return pump;
        }

        private static ChlorineMotor GetCholorineMotor(JObject o)
        {
            ChlorineMotor motor = new ChlorineMotor();
            motor.UUID = (string)o["PumpStation"]["Motor"]["Chlorine_Motor"]["uid"];
            motor.Controllable = (bool)o["PumpStation"]["Motor"]["Chlorine_Motor"]["Controllable"];
            motor.Auto = (bool)o["PumpStation"]["Motor"]["Chlorine_Motor"]["Auto"];
            return motor;
        }
        private static Sensor GetSensor(JObject o, int index, string rootname)
        {
            string type = rootname;
            string uid = (string)o["PumpStation"]["Sensor"][rootname][index]["uid"];
            string dataType = (string)o["PumpStation"]["Sensor"][rootname][index]["Data_Type"];
            string unit = (string) o["PumpStation"]["Sensor"][rootname][index]["Unit"];
            string model = (string) o["PumpStation"]["Sensor"][rootname][index]["Model"];
            string version = (string) o["PumpStation"]["Sensor"][rootname][index]["Version"];
            //Sensor sensor = GetCommonSensor(uid);
            return GetMatchedSensor(type, uid,dataType,unit,model,version);
        }
        private static Sensor GetMatchedSensor(string type, string uid,string dataType,string unit,string model,string version)
        {
            switch (type)
            {
                case "FT":
                    {
                        return CreateFlowSensor(uid, dataType,unit,model,version);
                    }
                case "ET":
                    {
                        return CreateEnergySensor(uid, dataType, unit, model, version);
                    }
                case "LT":
                    {
                        return CreateLevelSensor(uid, dataType, unit, model, version);
                    }
                case "PT":
                    {
                        return CreatePressureSensor(uid, dataType, unit, model, version);
                    }
                case "CPD":
                    {
                        return CreateChlorinationSensor(uid, dataType, unit, model, version);
                    }
                case "ACP":
                    {
                        return CreateAcPresenseDector(uid, dataType, unit, model, version);
                    }
                case "BV":
                    {
                        return CreateBatteryVoltageDetector(uid, dataType, unit, model, version);
                    }
            }

            return null;
        }
        private static Sensor CreateChlorinationSensor(string uid, string dataType, string unit, string model, string version)
        {
            ChlorinePresenceDetector sensor = new ChlorinePresenceDetector();
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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
        private static Sensor CreatePressureSensor(string uid, string dataType, string unit, string model, string version)
        {
            PressureSensor sensor = new PressureSensor();
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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
        private static Sensor CreateLevelSensor(string uid, string dataType, string unit, string model, string version)
        {
            LevelSensor sensor = new LevelSensor();
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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
        private static Sensor CreateEnergySensor(string uid, string dataType, string unit, string model, string version)
        {
            EnergySensor sensor = new EnergySensor();
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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
        private static Sensor CreateFlowSensor(string uid, string dataType, string unit, string model, string version)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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

        private static Sensor CreateAcPresenseDector(string uid, string dataType, string unit, string model, string version)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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

        private static Sensor CreateBatteryVoltageDetector(string uid, string dataType, string unit, string model, string version)
        {
            FlowSensor sensor = new FlowSensor();
            sensor.CumulativeValue = 0;
            sensor.MaximumValue = 0;
            sensor.CurrentValue = "0";
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
            sensor.CurrentValue = "0";
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
