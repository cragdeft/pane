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
            
            string loggedAt = o["SensorLoggedAt"].ToString();

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
    }
}
