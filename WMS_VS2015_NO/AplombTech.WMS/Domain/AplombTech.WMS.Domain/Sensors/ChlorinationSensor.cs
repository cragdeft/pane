using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("ChlorinationSensors")]
    public class ChlorinationSensor : Sensor
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
