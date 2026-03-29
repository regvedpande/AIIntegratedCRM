using AIIntegratedCRM.Domain.Entities;
using AIIntegratedCRM.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace AIIntegratedCRM.UnitTests.Infrastructure;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:SecretKey"]      = "TestSecretKeyThatIsAtLeast32CharactersLong!",
                ["JwtSettings:Issuer"]         = "TestIssuer",
                ["JwtSettings:Audience"]       = "TestAudience",
                ["JwtSettings:ExpiryMinutes"]  = "60"
            })
            .Build();

        _tokenService = new TokenService(config);
    }

    [Fact]
    public void GenerateAccessToken_Returns_Readable_JWT()
    {
        var user = MakeUser();
        var token = _tokenService.GenerateAccessToken(user);

        token.Should().NotBeNullOrEmpty();
        new JwtSecurityTokenHandler().CanReadToken(token).Should().BeTrue();
    }

    [Fact]
    public void GenerateAccessToken_Contains_TenantId_Claim()
    {
        var user  = MakeUser();
        var token = _tokenService.GenerateAccessToken(user);
        var jwt   = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwt.Claims.Should().Contain(c => c.Type == "tenantId" && c.Value == user.TenantId.ToString());
    }

    [Fact]
    public void GenerateAccessToken_Issuer_Matches_Config()
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_tokenService.GenerateAccessToken(MakeUser()));
        jwt.Issuer.Should().Be("TestIssuer");
    }

    [Fact]
    public void GenerateRefreshToken_Returns_64_Byte_Base64()
    {
        var rt    = _tokenService.GenerateRefreshToken();
        rt.Should().NotBeNullOrEmpty();
        Convert.FromBase64String(rt).Length.Should().Be(64);
    }

    // ── helpers ────────────────────────────────────────────────────
    private static AppUser MakeUser() => new()
    {
        Id        = Guid.NewGuid(),
        TenantId  = Guid.NewGuid(),
        FirstName = "Test",
        LastName  = "User",
        Email     = "test@example.com",
        Role      = "Admin"
    };
}
