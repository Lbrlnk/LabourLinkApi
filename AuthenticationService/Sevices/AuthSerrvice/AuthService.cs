using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Enums;
using AuthenticationService.Helpers.JwtHelper;
using AuthenticationService.Models;
using AuthenticationService.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;

namespace AuthenticationService.Sevices.AuthSerrvice
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;

     


        public AuthService(IAuthRepository authRepository, IJwtHelper jwtHelper, IMapper mapper)
        {

            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;

           

        }


        public async Task<bool> RegisterAsync(RegistrationDto registrationDto)
        {
            try
            {
                if (registrationDto == null)
                {
                    throw new Exception("details cant be empty ");
                }

                if (await _authRepository.GetUserByEmailAsync(registrationDto.Email) != null)
                {
                    throw new Exception("Email already exist");
                }


                registrationDto.Password = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

                var user = _mapper.Map<User>(registrationDto);


                await _authRepository.AddUserAsync(user);

                var response = await _authRepository.UpdateDatabaseAsync();

                if (response)
                {
                    return true;
                }

                throw new Exception("internal server error , registration failed ");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }
        public async Task<(string accessToken, string refreshToken, bool isProfileCompleted, UserType userType)> LoginAsync(LoginDto loginDto)
        {

            try
            {


                var user = await _authRepository.GetUserByEmailAsync(loginDto.email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.password, user.Password))
                {
                    throw new Exception("Invalid username or password");
                }

                if (!user.IsActive)
                {
                    throw new Exception("You are blocked");
                }

                var accessToken = _jwtHelper.GenerateToken(user);
                var refreshToken = _jwtHelper.GenerateRefreshToken();

               
                await _authRepository.SaveRefreshTokenAsync(user.UserId, refreshToken, DateTime.UtcNow.AddMonths(1));

                return (accessToken, refreshToken, user.IsProfileCompleted, user.UserType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in login : {ex.Message}", ex);
            }
        }


        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            
            var storedToken = await _authRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token. please login");
            }


            var user = await _authRepository.GetUserByIdAsync(storedToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            
            string newAccessToken = _jwtHelper.GenerateToken(user);

            return newAccessToken;  
        }

        public async Task<bool?> IsprofileCompleted(Guid userId)
        {
            try
            {
                return await _authRepository.IsProfileCompleted(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

