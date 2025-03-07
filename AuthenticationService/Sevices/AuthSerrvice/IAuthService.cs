using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Enums;

namespace AuthenticationService.Sevices.AuthSerrvice
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationDto registrationDto);
        Task<(string accessToken, string refreshToken, bool isProfileCompleted, UserType userType)> LoginAsync(LoginDto loginDto); 
        Task<string?> RefreshTokenAsync(string refreshToken);
       

    }
}
