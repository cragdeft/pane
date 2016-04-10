using AplombTech.WMS.JsonParser.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.QueryModel.Devices;
using AplombTech.WMS.QueryModel.Sensors;

namespace AplombTech.WMS.JsonParser
{
    public class JsonManager
    {
        public static DateTime? GetSensorLoggedAtTime(string message)
        {
            JObject o = JObject.Parse(message);

            string loggedAt = o["PumpStation"]["LogDateTime"].ToString();

            try
            {
                DateTime loggesAtTime = Convert.ToDateTime(loggedAt);
                return loggesAtTime;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static DateTime? GetConfigurationLoggedAtTime(string message)
        {
            JObject o = JObject.Parse(message);

            string loggedAt = o["PumpStation"]["ConfigureDateTime"].ToString();

            try
            {
                DateTime loggesAtTime = Convert.ToDateTime(loggedAt);
                return loggesAtTime;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static int? GetSensorPumpStationID(string message)
        {
            JObject o = JObject.Parse(message);

            string pumpStationId = o["PumpStation"]["PumoStation_Id"].ToString();

            try
            {
                int stationId = Convert.ToInt32(pumpStationId);
                return stationId;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static SensorMessage GetSensorObject(string message)
        {
            JObject o = JObject.Parse(message);

            SensorMessage sensorObject = new SensorMessage();

            sensorObject.PumpStationId = GetSensorPumpStationID(message);
            sensorObject.SensorLoggedAt = GetSensorLoggedAtTime(message);

            for (int i = 0; i < 5; i++)
            {
                sensorObject.Sensors.Add(GetSensorData(o, i));
            }
            //o["Sensor"][0]["uid"],o["Sensor"][0]["value"]

            return sensorObject;
        }

        public static ConfigurationMessage GetConfigurationObject(string message)
        {
            JObject o = JObject.Parse(message);

            ConfigurationMessage configurationObject = new ConfigurationMessage();

            configurationObject.PumpStationId = GetSensorPumpStationID(message);
            configurationObject.ConfigurationLoggedAt = GetConfigurationLoggedAtTime(message);

            for (int i = 0; i < o["PumpStation"]["Camera"].Count(); i++)
            {
                configurationObject.Cameras.Add(GetCamera(o, i));
            }

            for (int i = 0; i < o["PumpStation"]["Sensor"].Count(); i++)
            {
                configurationObject.Sensors.Add(GetSensor(o, i));
            }

            configurationObject.Router = (GetRouter(o));
            configurationObject.Pump = GetPump(o);

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
            router.UUID = (string)o["PumpStation"]["Router"]["uid"];
            router.MACAddress = (string)o["PumpStation"]["Router"]["mac_id"];
            router.IP = (string)o["PumpStation"]["Router"]["ip"];
            router.Port = (int)o["PumpStation"]["Router"]["port"];

            return router;
        }

        private static Pump GetPump(JObject o)
        {
            Pump pump = new Pump();
            pump.UUID = (string)o["PumpStation"]["Pump"]["uid"];
            return pump;
        }

        private static Sensor GetSensor(JObject o, int index)
        {
            string type = (string)o["PumpStation"]["Sensor"][index]["type"];
            string uid = (string)o["PumpStation"]["Sensor"][index]["uid"];
            //Sensor sensor = GetCommonSensor(uid);
            return GetMatchedSensor(type,uid);
        }

        private static Sensor GetMatchedSensor(string type,string uid)
        {
            switch (type)
            {
                case "FT":
                    {
                        FlowSensor sensor = new FlowSensor();
                        sensor.CumulativeValue = 0;
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
                case "ET":
                    {
                        EnergySensor sensor = new EnergySensor();
                        sensor.CumulativeValue = 0;
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
                case "LT":
                    {
                        LevelSensor sensor = new LevelSensor();
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
                case "PT":
                    {
                        PressureSensor sensor = new PressureSensor();
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
                case "CT":
                    {
                        ChlorinationSensor sensor = new ChlorinationSensor();
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

            return null;
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
