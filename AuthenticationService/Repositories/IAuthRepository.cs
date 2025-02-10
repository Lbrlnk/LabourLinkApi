using AuthenticationService.Models;

namespace AuthenticationService.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task AddLabour(Labour lbr);
        Task AddEmployer(Employer employer);

        Task<bool> UpdateDatabase();

       // Task<bool> GetUserByPhoneAsync(string phone);

        // pending

        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);

        Task<User?> GetUserByIdAsync(Guid userId);




    }
}
