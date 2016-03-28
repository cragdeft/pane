using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.DWasa.Utility.Enums;

namespace AplombTech.DWasa.Model.Models
{
    public class CommandJson : Repository.Pattern.Ef6.Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CommandJsonString { get; set; }
        public bool IsProcessed { get; set; }
        public CommandType CommandType { get; set; }
        public string ProcessFailReason { get; set; }

        #region  Complex Properties
        public AuditFields AuditField { get; set; }
        #endregion
    }
}
