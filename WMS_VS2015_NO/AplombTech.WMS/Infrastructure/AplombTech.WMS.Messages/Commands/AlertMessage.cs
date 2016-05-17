using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class AlertMessage : ICommand
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string PumpStationName { get; set; }
        public decimal MinimumValue { get; set; }
        public decimal Value { get; set; }
        public int AlertMessageType { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
