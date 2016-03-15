using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.DWasa.Model.Models
{
    public class Router:Device
    {
        #region  Navigation Properties
        public virtual Zone Zone { get; set; }
        #endregion
    }
}
