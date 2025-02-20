using AuthenticationService.Dtos.AuthenticationDtos;

namespace AuthenticationService.Sevices.AuthSerrvice
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationDto registrationDto);
        Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto loginDto); 
        Task<string?> RefreshTokenAsync(string refreshToken);
       

    }
}
