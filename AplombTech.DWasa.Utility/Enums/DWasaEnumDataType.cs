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
        SensorData,
        Feedback
    }

    public enum NetworkType
    {
        Gsm,
        BroadBand
    }

    public enum SensorType
    {
        FT=1,
        CT=2,
        EM=3,
        PT=4,
        LT=5
    }

    public enum ReportType
    {
        Hourly=1,
        Daily=2,
        Weekly=3,
        Monthly=4,
        Realtime=5
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
