using System.Security.Claims;

namespace ApiAuthentication.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateJwtToken(string email, string userID, bool rememberMe);
    string GenerateRefreshTokenJwt(string email, string userID, bool rememberMe);
    bool IsRefreshTokenValid(string token);
    (bool? isValid, bool? isExpired, ClaimsPrincipal principal) ValidateToken(string token);
    ClaimsPrincipal GetClaimsFromToken(string token);
    string GetClaimValue(string token, string claimType);
}
