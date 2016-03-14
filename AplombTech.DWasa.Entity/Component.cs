using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public abstract class Component
    {
        public virtual string Name { get; set; }
        public virtual string Location { get; set; }
        public virtual string Address { get; set; }

        public virtual void Add(Component component)
        {
            throw new Exception("Unable to add to this component.");
        }

        protected Component(string name, string location, string address)
        {
            Name = name;
            Location = location;
            Address = address;
        }
    }
}
