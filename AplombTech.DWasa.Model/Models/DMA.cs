using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplombTech.DWasa.Model.Models
{
    public class DMA : Area
    {
        #region Primitive Properties
       
        #endregion

        #region  Complex Properties
        #endregion

        #region  Navigation Properties

        public virtual Zone Zone { get; set; }
        #endregion
        
    }
}
