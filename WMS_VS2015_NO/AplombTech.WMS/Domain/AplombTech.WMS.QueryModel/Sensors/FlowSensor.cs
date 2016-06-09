using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Sensors
{
    [Table("FlowSensors")]
    public class FlowSensor : Sensor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Flow Sensor";

            t.Append(title);

            return t.ToString();
        }

        [MemberOrder(30), Required, Disabled]
        [DisplayName("Total")]
        public virtual decimal CumulativeValue { get; set; }
        //[MemberOrder(80)]
        //public virtual Unit Unit { get; set; }
    }
}
