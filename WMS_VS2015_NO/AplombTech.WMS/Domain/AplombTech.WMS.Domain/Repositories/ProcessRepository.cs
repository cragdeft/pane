using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Repositories
{
    public class ProcessRepository : AbstractFactoryAndRepository
    {
        public void ParseNStoreSensorData(SensorDataLog dataLog)
        {
            //var data = "{"name":"masnun","email":["masnun@gmail.com","masnun@leevio.com"],"websites":{"home page":"http:\/\/masnun.com","blog":"http:\/\/masnun.me"}}"
            //JObject o = JObject.Parse(data);
            //Console.WriteLine("Name: " + o["name"]);
            //Console.WriteLine("Email Address[1]: " + o["email"][0]);
            //Console.WriteLine("Email Address[2]: " + o["email"][1]);
            //Console.WriteLine("Website [home page]: " + o["websites"]["home page"]);
            //Console.WriteLine("Website [blog]: " + o["websites"]["blog"]);

            if (dataLog.ProcessingStatus == SensorDataLog.ProcessingStatusEnum.None)
            {

            }
        }

        public SensorDataLog LogSensorData(string topic, string message)
        {
            DateTime? LoggedAtTime = JsonManager.GetSensorLoggedAtTime(message);

            if (LoggedAtTime == null) return null;

            SensorDataLog sensorLogData = GetSensorLogData(topic, (DateTime)LoggedAtTime);

            if (sensorLogData == null)
            {
                SensorDataLog data = CreateLog(topic, message, (DateTime)LoggedAtTime);
                return data;
            }

            return sensorLogData;
        }

        private SensorDataLog GetSensorLogData(string topic, DateTime loggedAtSensor)
        {
            SensorDataLog dataLog = Container.Instances<SensorDataLog>().Where(w => w.Topic == topic && w.LoggedAtSensor == loggedAtSensor).FirstOrDefault();

            return dataLog;
        }

        private SensorDataLog CreateLog(string topic, string message, DateTime loggedAtSensor)
        {
            SensorDataLog data = Container.NewTransientInstance<SensorDataLog>();

            data.Topic = topic;
            data.Message = message;
            data.MessageReceivedAt = DateTime.Now;
            data.LoggedAtSensor = loggedAtSensor;
            data.ProcessingStatus = SensorDataLog.ProcessingStatusEnum.None;

            Container.Persist(ref data);

            return data;
        }

    }
}
