using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Repositories.LabourWithinEmployer;
using ProfileService.Services.JobPostServiceClientService;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrefferedController : ControllerBase
    {
        private readonly JobPostServiceClient _jobpost;
        private readonly IEmployerLabour _labour;
        public PrefferedController(JobPostServiceClient jobPost,IEmployerLabour labour)
        {
            _jobpost = jobPost;
            _labour = labour;
        }
        [HttpGet("getthejob")]
        public async Task<IActionResult> GetJobPostByLabour()
        {
			if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}
			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var userId))
			{
				return BadRequest("Invalid UserId format.");
			}
            var res =await _jobpost.GetPrefferedJobposts(userId);
            return Ok(res);
		}
		[HttpGet("getthelabourbyemployer")]
		public async Task<IActionResult> GetLabourByEmployer([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}

			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var userId))
			{
				return BadRequest("Invalid UserId format.");
			}

			var res = await _labour.GetLabourByEmployerMun(userId, pageNumber, pageSize);
			return Ok(res);
		}

	}
}
