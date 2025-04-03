using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Services.EmployerService;
using Sprache;

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
        [HttpGet("my-details")]
        public async Task<IActionResult> GetMydetails()
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
               var result =  await _employerService.GetEmployerDetails(userId);
                if(result == null)
                {
                    return BadRequest("user not found");

                }
                return Ok(result);

            }catch(Exception ex)
            {
                throw new Exception($" error in retriving Employer{ex.InnerException.Message ?? ex.Message}");
            }
        }
        [HttpGet("getallemployers")]
        public async Task<IActionResult> GetAllEmployers()
        {
            var res = await _employerService.GetAllEmployers();
            if (res.StatusCode == 200)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
        [HttpGet("getthecountofemployers")]
        public async Task<IActionResult> GetCount()
        {
            var res = await _employerService.CountEmployers();
            if (res.StatusCode == 200)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
		[HttpGet("getemployerbyid")]
		public async Task<IActionResult> GetEmployerById([FromQuery] Guid userId)
		{
			try
			{
				var result = await _employerService.GetEmployerDetails(userId);
				if (result == null)
				{
					return BadRequest("user not found");

				}
				return Ok(result);

			}
			catch (Exception ex)
			{
				throw new Exception($" error in retriving Employer{ex.InnerException.Message ?? ex.Message}");
			}
		}
	}
}
