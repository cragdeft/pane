using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Model.Enums;
using Repository.Pattern.Ef6;

namespace AplombTech.DWasa.Model.Models
{
    public class DataLog : Entity
    {
        #region Primitive Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DataLogId { get; set; }
        public string Production { get; set; }
        public string Energy { get; set; }
        public string Pressure { get; set; }
        public string WaterLevel { get; set; }
        public string Clorination { get; set; }
        public DateTime LogDateTime { get; set; }
        public string CheckSum { get; set; }
        public NetworkType NetworkType { get; set; }

        #endregion

        #region Complex Properties
        public AuditFields AuditField { get; set; }
        #endregion
    }
}
