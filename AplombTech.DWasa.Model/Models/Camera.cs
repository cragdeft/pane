using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    [Table("Cameras")]
    public class Camera:Device
    {
        public string Url { get; set; }
        public string UId { get; set; }
    }
}
