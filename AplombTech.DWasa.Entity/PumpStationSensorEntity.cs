using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;

namespace AplombTech.DWasa.Entity
{
    public class PumpStationSensorEntity: PumpStationDeviceEntity
    {
        public SensorEntity Sensor{ get; set; }
        [Display(Name = "Sensor")]
        [Required(ErrorMessage = "Sensor is required")]
        public SensorType SensorType { get; set; }
    }
}
