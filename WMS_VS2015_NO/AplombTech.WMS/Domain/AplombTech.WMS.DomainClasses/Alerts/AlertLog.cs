using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Alerts
{
  public class AlertLog
  {
    #region Primitive Properties
    [Key, NakedObjectsIgnore]
    public virtual int AlertLogId { get; set; }
    [StringLength(50)]
    [Optionally]
    public virtual string ReciverEmail { get; set; }
    [StringLength(50)]
    [Optionally]
    public virtual string ReciverMobileNo { get; set; }
    [Optionally]
    public int AlertMessageType { get; set; }
    [Optionally]
    public DateTime MessageDateTime { get; set; }
    [Optionally]
    public int AlertGereratedObjectId { get; set; }
    #endregion

  }
}
