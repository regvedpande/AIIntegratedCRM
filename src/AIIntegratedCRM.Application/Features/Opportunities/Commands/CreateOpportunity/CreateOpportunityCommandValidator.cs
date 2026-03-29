using FluentValidation;

namespace AIIntegratedCRM.Application.Features.Opportunities.Commands.CreateOpportunity;

public class CreateOpportunityCommandValidator : AbstractValidator<CreateOpportunityCommand>
{
    public CreateOpportunityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Probability).InclusiveBetween(0, 100);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}
