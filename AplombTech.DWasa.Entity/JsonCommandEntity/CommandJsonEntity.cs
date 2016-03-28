using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;

namespace AplombTech.DWasa.Entity.JsonCommandEntity
{
    public class CommandJsonEntity
    {
        public string CommandJsonString { get; set; }
        public bool IsProcessed { get; set; }
        public CommandType CommandType { get; set; }
        public string ProcessFailReason { get; set; }
    }
}
