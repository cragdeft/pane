using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class SummaryGenerationMessage : ICommand
    {
        public int SensorId { get; set; }
        public string SensorUUID { get; set; }
        public decimal Value { get; set; }
        public DateTime DataLoggedAt { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
