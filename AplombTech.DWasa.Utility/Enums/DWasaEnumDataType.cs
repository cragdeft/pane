using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Utility.Enums
{
    public enum CommandType
    {
        Command,
        Configuration,
        Feedback
    }

    public enum NetworkType
    {
        Gsm,
        BroadBand
    }

    public enum SensorType
    {
        WaterLevel,
        Cholorination,
        Energy,
        Production,
        Pressure
    }
}
