using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.ScoreLead;

public record ScoreLeadCommand(Guid LeadId) : IRequest<Result<ScoreLeadResult>>;

public record ScoreLeadResult(decimal Score, string Reason, string[] KeyFactors);
