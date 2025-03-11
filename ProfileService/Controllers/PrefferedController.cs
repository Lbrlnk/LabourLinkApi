using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Repositories.LabourPrefferedRepositorys;
using ProfileService.Repositories.LabourWithinEmployer;
namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrefferedController : ControllerBase
    {
        private readonly ILabourPrefferedRepository _repository;
        private readonly IEmployerLabour _labourrepo;
        public PrefferedController(ILabourPrefferedRepository repository,IEmployerLabour employerLabour)
        {
            _repository = repository;
            _labourrepo = employerLabour;
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
        [HttpGet("employerprefferedlabour")]
        public async Task<IActionResult> GetEmployerPrefferedLabour()
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
            var res =await _labourrepo.GetLabourByEmployerMun(userId);
            if (res.StatusCode == 200)
            {
                return Ok(res);
            }else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }
            return BadRequest();
            
		}
    }
}
