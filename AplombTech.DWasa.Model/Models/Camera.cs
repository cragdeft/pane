using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    public class Camera:Device
    {
        public string Url { get; set; }
        public string UId { get; set; }
    }
}
