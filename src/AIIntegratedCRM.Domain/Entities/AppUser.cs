using AIIntegratedCRM.Domain.Common;

namespace AIIntegratedCRM.Domain.Entities;

public class AppUser : BaseEntity
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Sales";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public string? TimeZone { get; set; } = "UTC";
    public string? Language { get; set; } = "en";

    public string FullName => $"{FirstName} {LastName}".Trim();
}
