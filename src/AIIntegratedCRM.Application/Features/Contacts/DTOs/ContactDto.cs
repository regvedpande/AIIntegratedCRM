namespace AIIntegratedCRM.Application.Features.Contacts.DTOs;

public class ContactDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public bool DoNotContact { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastContactedAt { get; set; }
}
