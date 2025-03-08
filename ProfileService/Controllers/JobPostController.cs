using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Repositories.LabourPrefferedRepositorys;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostController : ControllerBase
    {
        private readonly ILabourPrefferedRepository _repository;
        public JobPostController(ILabourPrefferedRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("labouprefferedjobpost")]
        public async Task<IActionResult> GetLabouprefferedjobpost()
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
            var res = await _repository.GetMatchingJobPostsAsync(userId);
            if (!res.Any())
            {
                return NotFound("there is no matching Jobpost");
            }
            return Ok(res);
		}
    }
}
