﻿using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Alerts
{
    public class AlertRecipient
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Life Cycle Methods
        // This region should contain any of the 'life cycle' convention methods (such as
        // Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

        public virtual void Persisting()
        {
            AuditFields.InsertedBy = Container.Principal.Identity.Name;
            AuditFields.InsertedDateTime = DateTime.Now;
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        public virtual void Updating()
        {
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int ReceipientId { get; set; }
        [Title]
        [MemberOrder(10)]
        [StringLength(100)]
        public virtual string Name { get; set; }
        [MemberOrder(30), Required]
        public virtual string MobileNo { get; set; }
        [MemberOrder(40), Required]
        public virtual string Email { get; set; }
        #endregion

        #region Complex Properties
        #region AuditFields (AuditFields)

        private AuditFields _auditFields = new AuditFields();

        [MemberOrder(250)]
        [Required]
        public virtual AuditFields AuditFields
        {
            get
            {
                return _auditFields;
            }
            set
            {
                _auditFields = value;
            }
        }

        public bool HideAuditFields()
        {
            return true;
        }
        #endregion
        #endregion

        #region  Navigation Properties
        [MemberOrder(20)]
        public virtual Designation Designation { get; set; }
        #endregion

        #region Collection Properties
        private ICollection<AlertType> _alertTypes = new List<AlertType>();
        [MemberOrder(50), Disabled]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(false, "AlertName")]
        public virtual ICollection<AlertType> AlertTypes
        {
            get
            {
                return _alertTypes;
            }
            set
            {
                _alertTypes = value;
            }
        }

        public virtual void AddToAlertTypes(AlertType value)
        {
            if (!(_alertTypes.Contains(value)))
            {
                _alertTypes.Add(value);
            }
        }

        public virtual void RemoveFromAlertTypes(AlertType value)
        {
            if (_alertTypes.Contains(value))
            {
                _alertTypes.Remove(value);
            }
        }

        public IList<AlertType> ChoicesRemoveFromAlertTypes(AlertType value)
        {
            return AlertTypes.ToList();
        }

        #endregion
    }
}
