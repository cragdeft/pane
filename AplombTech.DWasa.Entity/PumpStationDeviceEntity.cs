using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class PumpStationDeviceEntity
    {
        public PumpStationDeviceEntity()
        {
            PumpStationList = new List<PumpStationEntity>();
        }
        [Display(Name = "Pump Station")]
        [Required(ErrorMessage = "Please select Pump station")]
        public int PumpStationId { get; set; }
        public List<PumpStationEntity> PumpStationList { get; set; }
    }
}
