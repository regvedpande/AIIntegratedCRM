using AIIntegratedCRM.Domain.Common;
using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Domain.Entities;

public class WorkflowRule : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkflowTrigger Trigger { get; set; }
    public string ConditionsJson { get; set; } = "{}";
    public string ActionsJson { get; set; } = "[]";
    public bool IsActive { get; set; } = true;
    public int ExecutionCount { get; set; } = 0;
    public DateTime? LastExecutedAt { get; set; }
    public int Priority { get; set; } = 0;
}
