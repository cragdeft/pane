﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Sensors
{
    [Table("ChlorinePresenceDetectors")]
    public class ChlorinePresenceDetector : Sensor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Chlorination Sensor";

            t.Append(title);

            return t.ToString();
        }
    }
}
