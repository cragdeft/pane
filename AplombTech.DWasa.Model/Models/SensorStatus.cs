using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    public class SensorStatus : Repository.Pattern.Ef6.Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        #region  Complex Properties
        public AuditFields AuditField { get; set; }
        #endregion


        #region  Navigation Properties
        public virtual Device Device { get; set; }
        #endregion
    }
}
