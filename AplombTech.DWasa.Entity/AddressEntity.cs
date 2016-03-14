using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class AddressEntity
    {
        public string Street1 { get; private set; }
        public string Street2 { get; private set; }
        public string Zip { get; private set; }
        public string City { get; private set; }
    }
}
