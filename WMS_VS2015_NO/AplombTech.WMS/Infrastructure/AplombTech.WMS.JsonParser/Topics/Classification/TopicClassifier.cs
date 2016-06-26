using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Topics.Classification
{
    public class TopicClassifier : ITopicClassifier
    {
        public TopicType GetTopicType(string topic)
        {
            if (topic.Equals("wasa/configuration"))
                return TopicType.Configuration;
            else if (topic.Equals("wasa/sensor_data"))
                return TopicType.SensorData;
            else
                throw new InvalidTopicException();
        }
    }
}
