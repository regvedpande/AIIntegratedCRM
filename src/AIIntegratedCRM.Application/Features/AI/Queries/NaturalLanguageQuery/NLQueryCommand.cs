using AIIntegratedCRM.Application.Common.Models;
using MediatR;

namespace AIIntegratedCRM.Application.Features.AI.Queries.NaturalLanguageQuery;

public record NLQueryCommand(string Query) : IRequest<Result<string>>;
