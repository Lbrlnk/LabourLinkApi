using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Helpers.CloudinaryHelper;
using AuthenticationService.Helpers.JwtHelper;
using AuthenticationService.Models;
using AuthenticationService.Repositories;
using AutoMapper;
using System.Text.RegularExpressions;

namespace AuthenticationService.Sevices.AuthSerrvice
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        private readonly ICloudinaryHelper _cloudinary;
        //private readonly ILogger<AuthService> _logger;


        public AuthService(IAuthRepository authRepository, IJwtHelper jwtHelper ,IMapper mapper , ICloudinaryHelper cloudinary )
        {

            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
            _cloudinary = cloudinary;
            //_logger = logger;

        }

        //public async Task<bool> RegisterAsync<T>(T registerDto)
        //{
        //    try
        //    {

        //        //if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        //        if (await _authRepository.GetUserByEmailAsync(registerDto.Email) != null)
        //        {
        //            throw new Exception("Email already in use");
        //        }




        //        registerDto.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        //        var user = _mapper.Map<User>(registerDto);
        //        user.PasswordHash = registerDto.Password;



        //        Console.WriteLine($"Hashed Password: {user.PasswordHash}");

        //        await _authRepository.AddUserAsync(user);
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"database error:{ex.InnerException?.Message ?? ex.Message}");
        //        throw;
        //    }
        //}






        public async Task<bool> RegisterAsync<T>(T registerDto, LabourProfilePhotoDto profilePhoto)
        {
            try
            {
                var email = string.Empty;
                var passwordHash = string.Empty;
                

                if (registerDto is LabourRegistrationDto labourDto)
                {
                    // If necessary, assign values specific to Labour
                    //user.FullName = labourDto.FullName;
                    //user.PhoneNumber = labourDto.PhoneNumber;
                    // Set other properties specific to Labour

                    email = labourDto.Email;
                    passwordHash = labourDto.PasswordHash;
                    if (await _authRepository.GetUserByEmailAsync(email) != null)
                    {
                        throw new Exception("Email already in use");
                    }

                    passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordHash);

                    User user  = new User {
                        UserId = Guid.NewGuid(),
                        Email = email,
                        PasswordHash = passwordHash,
                        UserType = Enums.UserType.Labour,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsProfileCompleted = false,

                    };

                    await _authRepository.AddUserAsync(user);

                  var ProfilePhotoUrl = await _cloudinary.UploadLabourProfileImageAsyn(profilePhoto.photo);

                    Labour labour = new Labour
                    {
                        LabourId = Guid.NewGuid(),
                        UserId = user.UserId,
                        FullName = labourDto.FullName,
                        PhoneNumber = labourDto.PhoneNumber,
                        PreferedTime = labourDto.PreferedTime,
                        ProfilePhotoUrl = ProfilePhotoUrl,
                        IsActive = true
                    };

                    await _authRepository.AddLabour(labour);

                   return   await _authRepository.UpdateDatabase();


                }


                if (registerDto is EmployerRegistrationDto employerDto)
                {
                    email = employerDto.Email;
                    passwordHash = employerDto.PasswordHash;
                    if (await _authRepository.GetUserByEmailAsync(email) != null)
                    {
                        throw new Exception("Email already in use");
                    }

                    passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordHash);



                    User user = new User
                    {
                        UserId = Guid.NewGuid(),
                        Email = email,
                        PasswordHash = passwordHash,
                        UserType = Enums.UserType.Labour,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsProfileCompleted = true,

                    };

                    await _authRepository.AddUserAsync(user);

                    Employer employer = new Employer
                    {
                        EmployerId = Guid.NewGuid(),
                        UserId = user.UserId,
                        FullName = employerDto.FullName,
                        PhoneNumber = employerDto.PhoneNumber,
                        PreferedMuncipalityId = employerDto.PreferedMuncipalityId,

                    };

                    await _authRepository.AddEmployer(employer);

                   return  await _authRepository.UpdateDatabase();




                }


                //// Check if the email already exists in the database
                //if (await _authRepository.GetUserByEmailAsync(registerDto.Email) != null)
                //{
                //    throw new Exception("Email already in use");
                //}

                //// Hash the password before storing it
                //registerDto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.PasswordHash);

                //// Map the DTO to the User entity
                //var user = _mapper.Map<User>(registerDto);
                //user.PasswordHash = registerDto.PasswordHash;

                //Console.WriteLine($"Hashed Password: {user.PasswordHash}");

                // Handle LabourRegistrationDto-specific logic
                //if (registerDto is LabourRegistrationDto labourDto)
                //{
                //    // If necessary, assign values specific to Labour
                //    user.FullName = labourDto.FullName;
                //    user.PhoneNumber = labourDto.PhoneNumber;
                //    // Set other properties specific to Labour
                //}

                //// Handle EmployerRegistrationDto-specific logic
                //if (registerDto is EmployerRegistrationDto employerDto)
                //{
                //    // If necessary, assign values specific to Employer
                //    user.FullName = employerDto.FullName;
                //    user.PhoneNumber = employerDto.PhoneNumber;
                //    // Set other properties specific to Employer
                //}

                // Save the user to the repository
                //await _authRepository.AddUserAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }






        public async Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto loginDto)
        {
            // Generate tokens
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            if (!user.IsActive) 
            {
                throw new Exception("You are blocked");
            }

            var accessToken = _jwtHelper.GenerateToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            // Saving the refresh token in the database
            await _authRepository.SaveRefreshTokenAsync(user.UserId, refreshToken, DateTime.UtcNow.AddMonths(1));

            return (accessToken, refreshToken);
        }


        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            // Retrieve the refresh token from the database
            var storedToken = await _authRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token. please login");
            }

            // Retrieve user details associated with the refresh token

            // check if this correct can we check user id from the refresh token will share an example refresh token 
            var user = await _authRepository.GetUserByIdAsync(storedToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Generate new access token
            string newAccessToken = _jwtHelper.GenerateToken(user);

            return newAccessToken;  // Returning new access token
        }


    }
}
