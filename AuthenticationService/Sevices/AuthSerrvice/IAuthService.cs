using AuthenticationService.Dtos.AuthenticationDtos;

namespace AuthenticationService.Sevices.AuthSerrvice
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync<T>(T registerDto, LabourProfilePhotoDto profilePhoto);
        Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto loginDto); 
        Task<string?> RefreshTokenAsync(string refreshToken);
       

    }
}
