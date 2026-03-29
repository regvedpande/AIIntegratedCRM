using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.AI.Commands.GenerateEmail;

public class GenerateEmailCommandHandler : IRequestHandler<GenerateEmailCommand, Result<string>>
{
    private readonly IAIService _aiService;

    public GenerateEmailCommandHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<Result<string>> Handle(GenerateEmailCommand request, CancellationToken cancellationToken)
    {
        var emailRequest = new EmailGenerationRequest(
            request.Tone, request.Purpose, request.RecipientName,
            request.RecipientCompany, request.Context, request.SenderName);

        var email = await _aiService.GenerateEmailAsync(emailRequest, cancellationToken);
        return Result<string>.Success(email);
    }
}
