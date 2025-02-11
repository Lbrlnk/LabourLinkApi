using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Sevices.AuthSerrvice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
        //{
        //    try
        //    {

        //        if (registerDto == null)
        //        {
        //            return BadRequest(new { message = "form are incomplete" });
        //        }

        //        var result = await _authService.RegisterAsync(registerDto);
        //        if (result)
        //        {
        //            return Ok(new { message = "Registration Successful" });
        //        }
        //        else
        //        {
        //            return BadRequest(new { message = "Registraion failed" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        //    }
        //}



        [HttpPost("labour/registeration")]
        public async Task<IActionResult> LabouRegistration([FromForm] LabourRegistrationDto lbrRgstrDto, [FromForm]  LabourProfilePhotoDto profilePhoto)
        {

            try
            {

            if (lbrRgstrDto == null)
            {
                return BadRequest(new { message = "complete the form before submitting" });
            }
            var result = await _authService.RegisterAsync(lbrRgstrDto , profilePhoto);
            if (result)
            {
                return Ok(new { message = "Registration Successful" });
            }
            else
            {
                return BadRequest(new { message = "Registraion failed" });
            }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }


        }

        [HttpPost("employer/registration")]
        public async Task<IActionResult> EmployerRegister([FromBody] EmployerRegistrationDto emplyrRgstrDto)
        {

            try
            {

                if (emplyrRgstrDto == null)
                {
                    return BadRequest(new { message = "complete the form before submitting" });
                }
                var result = await _authService.RegisterAsync(emplyrRgstrDto , null);
                if (result)
                {
                    return Ok(new { message = "Registration Successful" });
                }
                else
                {
                    return BadRequest(new { message = "Registraion failed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logindto)
        {
            try
            {
                if (logindto == null || string.IsNullOrEmpty(logindto.Username) || string.IsNullOrEmpty(logindto.Password))
                {
                    return BadRequest(new { message = "Username and Password are required" });
                }

                var (accessToken, refreshToken) = await _authService.LoginAsync(logindto);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }




                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Use Secure only in non-local environments
                    SameSite = SameSiteMode.None, // Required for cross-origin cookies
                    Expires = DateTime.UtcNow.AddMinutes(15) // Adjust for access/refresh token
                };

                Response.Cookies.Append("accessToken", accessToken, cookieOptions);

                cookieOptions.Expires = DateTime.UtcNow.AddMonths(1); // Update for refresh token
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);


                return Ok(new { message = "Login Successful", accessToken, refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("refresh/token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { message = "Refresh token is missing" });
                }

                var newAccessToken = await _authService.RefreshTokenAsync(refreshToken);

                // Set new access token in cookies
                Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });

                return Ok(new { message = "Token refreshed successfully", accessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}
