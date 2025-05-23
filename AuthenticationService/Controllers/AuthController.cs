﻿using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Sevices.AuthSerrvice;
using Microsoft.AspNetCore.Authorization;
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





        [HttpPost("labourlink/register")]
        public async Task<IActionResult> EmployerRegister([FromBody] RegistrationDto registrationDto)
        {

            try
            {

                if (registrationDto == null)
                {
                    return BadRequest(new { message = "complete the form before submitting" });
                }
                var result = await _authService.RegisterAsync(registrationDto);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }

        }



        [HttpPost("labourlink/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logindto)
        {
            try
            {
                if (logindto == null || string.IsNullOrEmpty(logindto.email) || string.IsNullOrEmpty(logindto.password))
                {
                    return BadRequest(new { message = "Username and Password are required" });
                }

                var (accessToken, refreshToken, isProfileCompleted, userType) = await _authService.LoginAsync(logindto);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }




                //var cookieOptions = new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true, // Use Secure only in non-local environments
                //    SameSite = SameSiteMode.None, // cross-origin cookies
                //    Expires = DateTime.UtcNow.AddMinutes(15)
                //};

                //Response.Cookies.Append("accessToken", accessToken, cookieOptions);

                //cookieOptions.Expires = DateTime.UtcNow.AddMonths(1);
                //Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                

                return Ok(new { message = "Login Successful", accessToken, refreshToken, isProfileCompleted, userType });
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
                //Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    Expires = DateTime.UtcNow.AddMinutes(15)
                //});

                return Ok(new { message = "Token refreshed successfully", accessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpPost("labourlink/logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("isProfileCompleted")]
        public async Task<IActionResult> IsProfileCompleted()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var result =  await _authService.IsprofileCompleted(userId);
            if(result == null)
            {
                return BadRequest("user not found");
            }
            return Ok(result);
        }

	}
}
