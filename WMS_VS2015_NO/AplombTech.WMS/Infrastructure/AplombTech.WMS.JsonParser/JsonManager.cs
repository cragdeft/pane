using AplombTech.WMS.JsonParser.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch(Exception ex)
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

            for (int i =1; i<5; i++)
            {
                sensorObject.Sensors.Add(GetSensorData(o,i));
            }
            //o["Sensor"][0]["uid"],o["Sensor"][0]["value"]

            return sensorObject;
        }
        private static SensorValue GetSensorData(JObject o, int index)
        {
            SensorValue data = new SensorValue();
            data.SensorUUID = (string) o["PumpStation"]["Sensor"][index]["uid"];
            data.Value = (string)o["PumpStation"]["Sensor"][index]["value"];


            return data;
        }
    }
}
