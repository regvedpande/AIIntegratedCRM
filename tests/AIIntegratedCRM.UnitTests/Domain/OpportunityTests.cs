using AIIntegratedCRM.Domain.Entities;
using AIIntegratedCRM.Domain.Enums;
using AIIntegratedCRM.Domain.Events;
using FluentAssertions;
using Xunit;

namespace AIIntegratedCRM.UnitTests.Domain;

public class OpportunityTests
{
    private static Opportunity Build(OpportunityStage stage = OpportunityStage.Prospecting) => new()
    {
        Id       = Guid.NewGuid(),
        TenantId = Guid.NewGuid(),
        Name     = "Test Deal",
        Stage    = stage
    };

    [Fact]
    public void ChangeStage_Should_Update_Stage()
    {
        var opp = Build();
        opp.ChangeStage(OpportunityStage.Qualification);
        opp.Stage.Should().Be(OpportunityStage.Qualification);
    }

    [Fact]
    public void ChangeStage_Should_Raise_OpportunityStageChangedEvent()
    {
        var opp = Build(OpportunityStage.Prospecting);
        opp.ChangeStage(OpportunityStage.Proposal);

        opp.DomainEvents.Should().HaveCount(1);
        var evt = opp.DomainEvents.First().Should().BeOfType<OpportunityStageChangedEvent>().Subject;
        evt.OldStage.Should().Be(OpportunityStage.Prospecting);
        evt.NewStage.Should().Be(OpportunityStage.Proposal);
    }

    [Fact]
    public void ChangeStage_To_ClosedWon_Should_Set_ActualCloseDate()
    {
        var opp = Build(OpportunityStage.Negotiation);
        opp.ChangeStage(OpportunityStage.ClosedWon);

        opp.ActualCloseDate.Should().NotBeNull();
        opp.ActualCloseDate!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ChangeStage_To_ClosedLost_Should_Set_LostReason()
    {
        var opp = Build(OpportunityStage.Negotiation);
        opp.ChangeStage(OpportunityStage.ClosedLost, "Budget constraints");

        opp.Stage.Should().Be(OpportunityStage.ClosedLost);
        opp.LostReason.Should().Be("Budget constraints");
    }
}
