using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class AddressEntity
    {
        [Display(Name = "Street 1")]
        public string Street1 { get;  set; }
        [Display(Name = "Street 2")]
        public string Street2 { get;  set; }
        public string Zip { get;  set; }
        public string City { get;  set; }
    }
}
