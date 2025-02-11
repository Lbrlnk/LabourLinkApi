using AuthenticationService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Helpers.JwtHelper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            try
            {
                // retreving configuration settings
                var secretKey = _configuration["JWT_SECRET_KEY"];
                var issuer = _configuration["JWT_ISSUER"];
                var audience = _configuration["JWT_AUDIENCE"];
                var expiryInHours = int.TryParse(_configuration["JwtExpiryInHours"], out var hours) ? hours : 2; 

                if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {

                    throw new Exception($"Missing JWT configuration values. SecretKey: {secretKey}, Issuer: {issuer}, Audience: {audience}");
                }

                // Generate the security key and credentials
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Define claims
                var claims = new[]
                {

                    new Claim(ClaimTypes.Role, user.UserType.ToString()), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JwtId
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
                    new Claim(ClaimTypes.Email, user.Email) 

                };

                // Create the token
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(expiryInHours),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception (you can implement a logger here)
                throw new Exception("An error occurred while generating the token.", ex);
            }
        }

        // pending
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        //public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        //{
        //    var secretKey = _configuration["JWT_SECRET_KEY"];
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = key,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidateLifetime = false // Allow expired tokens
        //        }, out _);

        //        return principal;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

    }
}
