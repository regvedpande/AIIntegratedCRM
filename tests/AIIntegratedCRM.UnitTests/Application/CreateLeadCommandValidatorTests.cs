using AIIntegratedCRM.Application.Features.Leads.Commands.CreateLead;
using FluentAssertions;
using Xunit;

namespace AIIntegratedCRM.UnitTests.Application;

public class CreateLeadCommandValidatorTests
{
    private readonly CreateLeadCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Should_Pass()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John",
            LastName  = "Doe",
            Email     = "john.doe@example.com"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("",     "Doe",  "john@example.com")]
    [InlineData("John", "",     "john@example.com")]
    [InlineData("John", "Doe",  "")]
    [InlineData("John", "Doe",  "not-an-email")]
    public void Validate_InvalidCommand_Should_Fail(string firstName, string lastName, string email)
    {
        var command = new CreateLeadCommand
        {
            FirstName = firstName,
            LastName  = lastName,
            Email     = email
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_FirstNameExceedingMaxLength_Should_Fail()
    {
        var command = new CreateLeadCommand
        {
            FirstName = new string('A', 101),
            LastName  = "Doe",
            Email     = "john@example.com"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
    }
}
