using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Opportunities.Commands.ChangeStage;

public record ChangeOpportunityStageCommand(Guid OpportunityId, OpportunityStage NewStage, string? Reason = null) : IRequest<Result>;
