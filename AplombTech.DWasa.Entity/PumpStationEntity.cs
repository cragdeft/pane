using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class PumpStationEntity:AreaEntity
    {
        #region Primitive Properties
        public DMAEntity DMA { get; set; }
        public virtual ICollection<DeviceEntity> DeviceList { get; set; }
        public virtual ICollection<WaterLevelSensorEntity> WaterLevelSensorList { get; set; }

        public virtual ICollection<ProductionSensorEntity> ProductionSensorList { get; set; }

        public virtual ICollection<PressureSensorEntity> PressureSensorList { get; set; }
        public virtual ICollection<EnergySensorEntity> EnergySensorList { get; set; }
        public virtual ICollection<CholorinationSensorEntity> CholorinationSensorList { get; set; }


        #endregion
    }
}
