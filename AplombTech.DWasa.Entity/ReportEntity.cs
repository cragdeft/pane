using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;

namespace AplombTech.DWasa.Entity
{
    public class ReportEntity
    {
        public ReportEntity()
        {
            PumpStation = new PumpStationDeviceEntity();
            Series = new List<ReportSeriesEntity>();
        }
        public string GraphTitle { get; set; }
        public List<ReportSeriesEntity> Series { get; set; }
        public string GraphSubTitle { get; set; }
        public string[] XaxisCategory { get; set; }
        [DisplayFormat(DataFormatString = "{0:dddd dd, MMMM, yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "To")]
        public DateTime ToDateTime { get; set; }
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime FromDateTime { get; set; }
        [Display(Name = "Period")]
        public ReportType ReportType { get; set; }

        public Month Month { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public PumpStationDeviceEntity PumpStation { get; set; }
        [Display(Name = "Sensor Type")]
        public SensorType SensorType { get; set; }

        public string Unit { get; set; }

        public int DeviceId { get; set; }
    }
}
