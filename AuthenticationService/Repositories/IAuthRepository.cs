using AuthenticationService.Models;

namespace AuthenticationService.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> UpdateDatabaseAsync();
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByIdAsync(Guid userId);

        Task<bool> MarkProfileAsCompleted(Guid userId);





    }
}
