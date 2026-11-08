using BBoxSoft.Domain.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBoxSoft.Domain.Models.Universe
{
    public class UnvTenantModel
    {
        public UnvTenantModel()
        {
            IdTenant = -1;
            Code = string.Empty;
            Name = string.Empty;
            Status = 1;
            IsEnabled = true;
            CreatedBy = -1;
            CreatedAt = Constants.DefaultDateTime;
            UpdatedBy = -1;
            UpdatedAt = Constants.DefaultDateTime;
            AuditAction = 1;
            WorkflowCode = Guid.Empty;
        }

        public UnvTenantModel(long idTenant, string code, string name, short status, bool isEnabled, long createdBy, DateTime createdAt, long updatedBy, DateTime updatedAt, short auditAction, Guid workflowCode)
        {
            IdTenant = idTenant;
            Code = code;
            Name = name;
            Status = status;
            IsEnabled = isEnabled;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            AuditAction = auditAction;
            WorkflowCode = workflowCode;
        }

        public long IdTenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public bool IsEnabled { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public short AuditAction { get; set; }
        public Guid WorkflowCode { get; set; }

    }

}
