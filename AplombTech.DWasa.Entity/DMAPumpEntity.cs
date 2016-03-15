using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class DMAPumpEntity
    {
        public int ZoneId { get; set; }
        [Required(ErrorMessage = "Please select DMA")]
        public int DMAId { get; set; }
        public List<ZoneEntity> ZoneList { get; set; }
        public List<DMAEntity> DMAList { get; set; }
        public PumpStationEntity PumpStationEntity { get; set; }
    }
}
