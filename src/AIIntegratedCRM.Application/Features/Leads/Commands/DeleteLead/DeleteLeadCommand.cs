using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Commands.DeleteLead;

public record DeleteLeadCommand(Guid Id) : IRequest<Result>;
