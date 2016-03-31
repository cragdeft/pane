using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Devices
{
    [Table("Pumps")]
    public class Pump : Device
    {
        [Title]
        [MemberOrder(20)]
        public virtual string ModelNo { get; set; }
        public virtual decimal Capacity { get; set; }
        public virtual int StaticWaterLevel { get; set; }
        public virtual string RemoveRemarks { get; set; }
        public bool HideRemoveRemarks()
        {
            if (IsRemoved)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public virtual bool IsRemoved { get; set; }
        public bool HideIsRemoved()
        {
            return true;
        }

        #region RemovePump (Action)
        [DisplayName("Remove Pump")]
        public void RemovePump(string remarks)
        {
            this.RemoveRemarks = remarks;
            this.IsRemoved = true;
        }
        public bool RemovePump()
        {
            if (this.IsRemoved)
                return true;

            return false;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("RemovePump");           
        }
        #endregion
    }
}
