using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages.Exceptions;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
    public class ConfigurationMessageParser : IDeviceMessageParser<ConfigurationMessage>
    {
        public ConfigurationMessage ParseMessage(string messageString)
        {
            try
            {
                ConfigurationMessage messageObject = JsonManager.GetConfigurationObject(messageString);

                return messageObject;
            }
            catch (Exception)
            {
                
                throw new ConfigurationMessageException();
            }
            
        }
    }
}
