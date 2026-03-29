using AIIntegratedCRM.Domain.Entities;
using AIIntegratedCRM.Domain.Enums;
using AIIntegratedCRM.Domain.Events;
using FluentAssertions;
using Xunit;

namespace AIIntegratedCRM.UnitTests.Domain;

public class LeadTests
{
    [Fact]
    public void Create_Lead_Should_Set_Properties_Correctly()
    {
        var tenantId  = Guid.NewGuid();
        var lead = Lead.Create("John", "Doe", "john.doe@example.com", tenantId, "admin@test.com");

        lead.FirstName.Should().Be("John");
        lead.LastName.Should().Be("Doe");
        lead.Email.Should().Be("john.doe@example.com");
        lead.TenantId.Should().Be(tenantId);
        lead.Status.Should().Be(LeadStatus.New);
        lead.IsDeleted.Should().BeFalse();
        lead.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_Lead_Should_Raise_LeadCreatedEvent()
    {
        var tenantId = Guid.NewGuid();
        var lead = Lead.Create("Jane", "Smith", "jane@example.com", tenantId, "system");

        lead.DomainEvents.Should().HaveCount(1);
        var evt = lead.DomainEvents.First().Should().BeOfType<LeadCreatedEvent>().Subject;
        evt.TenantId.Should().Be(tenantId);
        evt.Email.Should().Be("jane@example.com");
        evt.LeadId.Should().Be(lead.Id);
    }

    [Fact]
    public void Convert_Lead_Should_Update_Status_And_ConvertedAt()
    {
        var lead = Lead.Create("John", "Doe", "john@example.com", Guid.NewGuid(), "system");
        var contactId     = Guid.NewGuid();
        var accountId     = Guid.NewGuid();
        var opportunityId = Guid.NewGuid();

        lead.Convert(contactId, accountId, opportunityId);

        lead.Status.Should().Be(LeadStatus.Converted);
        lead.ConvertedAt.Should().NotBeNull();
        lead.ConvertedContactId.Should().Be(contactId);
        lead.ConvertedAccountId.Should().Be(accountId);
        lead.ConvertedOpportunityId.Should().Be(opportunityId);
    }

    [Fact]
    public void FullName_Should_Combine_First_And_Last_Name()
    {
        var lead = Lead.Create("John", "Doe", "john@example.com", Guid.NewGuid(), "system");
        lead.FullName.Should().Be("John Doe");
    }

    [Fact]
    public void ClearDomainEvents_Should_Remove_All_Events()
    {
        var lead = Lead.Create("John", "Doe", "john@example.com", Guid.NewGuid(), "system");
        lead.DomainEvents.Should().NotBeEmpty();

        lead.ClearDomainEvents();

        lead.DomainEvents.Should().BeEmpty();
    }
}
