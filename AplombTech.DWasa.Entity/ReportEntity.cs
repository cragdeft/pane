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
        public string GraphTitle { get; set; }
        public string Series { get; set; }
        public string GraphSubTitle { get; set; }
        public string[] XaxisCategory { get; set; }
        [Required(ErrorMessage = "Please select Start Date")]
        [DisplayFormat(DataFormatString = "{0:dddd dd, MMMM, yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "To")]
        public DateTime ToDateTime { get; set; }
        [Display(Name = "From")]
        public DateTime FromDateTime { get; set; }
        [Required(ErrorMessage = "Please select report type")]
        [Display(Name = "Type")]
        public ReportType ReportType { get; set; }

        public int DeviceId { get; set; }
    }
}
