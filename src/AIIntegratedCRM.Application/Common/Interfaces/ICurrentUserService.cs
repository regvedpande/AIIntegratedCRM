namespace AIIntegratedCRM.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string Email { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}
