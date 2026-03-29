using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Activities.Commands.SummarizeMeeting;

public record SummarizeMeetingCommand(Guid ActivityId, string Transcript) : IRequest<Result<string>>;
