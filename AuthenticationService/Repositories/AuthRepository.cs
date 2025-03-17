using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repositories
{

    public class AuthRepository : IAuthRepository
    {
        private readonly AuthenticationDbContext _context;

        public AuthRepository(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }


        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            Console.WriteLine($"Before saving: IsActived = {user.IsActive}");
            //await _context.SaveChangesAsync();
        }



        public async Task<bool> UpdateDatabaseAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }





        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
            }

            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = expiryDate
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }


        public async Task<bool> MarkProfileAsCompleted(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;

            user.IsProfileCompleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
