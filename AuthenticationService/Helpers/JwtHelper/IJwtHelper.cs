using AuthenticationService.Models;
using System.Security.Claims;

namespace AuthenticationService.Helpers.JwtHelper
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(); 
        //ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
