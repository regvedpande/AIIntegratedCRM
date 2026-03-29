using AIIntegratedCRM.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace AIIntegratedCRM.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("user.name+tag@sub.example.co.uk")]
    public void Create_ValidEmail_Should_Succeed(string address)
    {
        var email = Email.Create(address);
        email.Should().NotBeNull();
        email.Value.Should().Be(address.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-an-email")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-domain@")]
    public void Create_InvalidEmail_Should_Throw(string address)
    {
        var act = () => Email.Create(address);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Same_Email_Values_Are_Equal()
    {
        var a = Email.Create("test@example.com");
        var b = Email.Create("TEST@EXAMPLE.COM");
        a.Should().Be(b);
    }

    [Fact]
    public void Email_Implicit_Converts_To_String()
    {
        var email = Email.Create("test@example.com");
        string str = email;
        str.Should().Be("test@example.com");
    }
}
