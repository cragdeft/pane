using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplombTech.DWasa.Model.Models
{
    public class PumpStation : Area
    {
        #region Primitive Properties
        #endregion

        #region  Complex Properties
        public virtual DMA DMA { get; set; }
        #endregion
    }
}
