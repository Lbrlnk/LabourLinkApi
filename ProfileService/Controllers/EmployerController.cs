﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Services.EmployerService;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        public EmployerController(IEmployerService employeeService)
        {
            _employerService = employeeService;
        }
        [HttpPost("Complte/Profile")]
        public async Task<IActionResult> CompleteEmployerProfile([FromForm] CompleteEmployerProfileDto employerProfileDto)
        {
            try
            {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
           var result  = await _employerService.CompleteEmployerProfile(userId, employerProfileDto);
            return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

        }

        [HttpPatch("edit/Profile")]
        public async Task<IActionResult> EditEmployerProfile([FromForm] EditEmployerProfileDto editEmployerProfileDto)
        {
            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _employerService.UpdateEmployerProfile(userId, editEmployerProfileDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
