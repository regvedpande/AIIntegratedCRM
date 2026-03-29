using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.AI.Commands.GenerateEmail;

public record GenerateEmailCommand : IRequest<Result<string>>
{
    public string Tone { get; init; } = "Professional";
    public string Purpose { get; init; } = string.Empty;
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientCompany { get; init; } = string.Empty;
    public string? Context { get; init; }
    public string SenderName { get; init; } = string.Empty;
}
