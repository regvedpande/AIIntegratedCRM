using AIIntegratedCRM.Domain.Entities;

namespace AIIntegratedCRM.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(AppUser user);
    string GenerateRefreshToken();
    Guid? ValidateRefreshToken(string refreshToken);
}
