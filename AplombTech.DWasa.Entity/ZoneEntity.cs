using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public class ZoneEntity:AreaEntity
    {
        public ZoneEntity()
        {
            this.Address=new AddressEntity();
        }

        public virtual ICollection<DMAEntity> DMAList { get; set; }
    }
}
