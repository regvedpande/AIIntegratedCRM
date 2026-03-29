using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Leads.DTOs;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Leads.Queries.GetLeadById;

public record GetLeadByIdQuery(Guid Id) : IRequest<Result<LeadDto>>;
