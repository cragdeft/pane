using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Entity
{
    public abstract class CompositeComponent:Component
    {
        protected readonly List<Component> Components=new List<Component>();
        protected CompositeComponent(string name, string location, string address) : base(name, location, address)
        {
        }

        public override void Add(Component component)
        {
            Components.Add(component);
        }
    }
}
