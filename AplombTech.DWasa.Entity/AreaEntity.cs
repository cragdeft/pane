using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class AreaEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public AddressEntity Address { get; set; }
    }
}
