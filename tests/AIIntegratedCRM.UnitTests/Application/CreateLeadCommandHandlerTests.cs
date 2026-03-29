using AIIntegratedCRM.Application.Common.Interfaces;
using AIIntegratedCRM.Application.Features.Leads.Commands.CreateLead;
using AIIntegratedCRM.Domain.Entities;
using AIIntegratedCRM.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AIIntegratedCRM.UnitTests.Application;

public class CreateLeadCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext>  _mockContext;
    private readonly Mock<ICurrentUserService>    _mockCurrentUser;
    private readonly CreateLeadCommandHandler     _handler;
    private readonly List<Lead>                   _leads = new();

    public CreateLeadCommandHandlerTests()
    {
        _mockContext     = new Mock<IApplicationDbContext>();
        _mockCurrentUser = new Mock<ICurrentUserService>();

        _mockCurrentUser.Setup(u => u.TenantId).Returns(Guid.NewGuid());
        _mockCurrentUser.Setup(u => u.Email).Returns("test@example.com");
        _mockCurrentUser.Setup(u => u.IsAuthenticated).Returns(true);

        var mockSet = BuildMockDbSet(_leads);
        _mockContext.Setup(c => c.Leads).Returns(mockSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateLeadCommandHandler(_mockContext.Object, _mockCurrentUser.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_Returns_Success_With_Guid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "Alice",
            LastName  = "Johnson",
            Email     = "alice@example.com",
            Company   = "Tech Corp",
            Source    = LeadSource.Website
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Call_SaveChangesAsync_Once()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "Bob",
            LastName  = "Smith",
            Email     = "bob@example.com"
        };

        await _handler.Handle(command, CancellationToken.None);

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── helpers ────────────────────────────────────────────────────
    private static Mock<DbSet<T>> BuildMockDbSet<T>(List<T> source) where T : class
    {
        var q   = source.AsQueryable();
        var set = new Mock<DbSet<T>>();
        set.As<IQueryable<T>>().Setup(m => m.Provider).Returns(q.Provider);
        set.As<IQueryable<T>>().Setup(m => m.Expression).Returns(q.Expression);
        set.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(q.ElementType);
        set.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => q.GetEnumerator());
        set.Setup(m => m.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
           .Callback<T, CancellationToken>((e, _) => source.Add(e))
           .ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>)null!);
        return set;
    }
}
