using JobPostService.Dtos;
using JobPostService.Helpers.ApiResonse;
using JobPostService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPostService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private readonly IJobService _service;
		public JobController(IJobService service)
		{
			_service = service;
		}
		[Authorize(Roles = "Employer")]
		[HttpPost("createjobpost")]
		public async Task<IActionResult> PostAJob([FromForm] JobPostDto jobPostDto,IFormFile image)
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
			var response= await _service.AddNewPost(jobPostDto, image, userId);
			if (response.StatusCode == 201)
			{
				return Ok(response);
			}
			if(response.StatusCode == 400)
			{
				return BadRequest(response);
			}
		    return BadRequest(response);
		}

		[HttpGet("showallJobpost")]
		public async Task<IActionResult> ShowAllPost()
		{
			var res = await _service.GetJobPost();
			if (res.StatusCode == 200)
			{
				return Ok(res);
			}
				return NotFound(res);
		}
		[HttpGet("showallJobpostactive")]
		public async Task<IActionResult> ShowAllPostactive()
		{
			var res = await _service.GetJobPostactive();
			if (res.StatusCode == 200)
			{
				return Ok(res);
			}
			return NotFound(res);
		}
		[HttpGet("getjobpostbyid")]
		public async Task<IActionResult> GetJobpostbyid(Guid id)
		{
			var res=await _service.GetJobPostById(id);
			if(res.StatusCode == 200)
			{
				return Ok(res);
			}
			return NotFound(res);
		}
		[Authorize(Roles = "Employer")]
		[HttpPatch("updatejobpost")]
		public async Task<IActionResult> UpdateJobPost(Guid jobId,UpdatePostDto updatePostDto)
		{
			if (updatePostDto == null)
			{
				return BadRequest(new ApiResponse<string>(400, "Invalid request data."));
			}
			if(!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}

			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var userId))
			{
				return BadRequest("Invalid UserId format.");
			}
			var res = await _service.UpdateJobPost(updatePostDto, userId, jobId);
			if (res.StatusCode == 200)
			{
				return Ok(res);
			}
			if(res.StatusCode == 404)
			{
				return NotFound(res);
			}
			return BadRequest(res);
		}
		[Authorize(Roles = "Employer")]
		[HttpPatch("changestatus")]
		public async Task<IActionResult> UpdateStatus(string status, Guid jobid)
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
			var res=await _service.ChangeStatus(status, jobid, userId);
			if(res.StatusCode == 200)
			{
				return Ok(res);
			}else if(res.StatusCode == 400)
			{
				return BadRequest(res.Message);
			}else if(res.StatusCode ==404)
			{
				return BadRequest("There is no value in this jobid");
			}else if (res.StatusCode == 403)
			{
				return BadRequest("Unauthorized to update this job post");
			}
			return BadRequest(res);
		}
		[Authorize(Roles = "Employer")]
		[HttpGet("jobpostbyclient")]
		public async Task<IActionResult> ShowJobPostByClient()
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

			var res=await _service.GetJobPostByClientid(userId);
			if(res.StatusCode == 200)
			{
				return Ok(res);
			}else if (res.StatusCode == 404)
			{
				return NotFound(res);
			}
			return BadRequest(res);
		}
		[HttpGet("searchforjobpost")]
		public async Task<IActionResult> SearchJobPost(string searchparam)
		{
			var res =await _service.SearchJobPost(searchparam);
			if(res.StatusCode == 200)
			{
				return Ok(res);
			}else if (res.StatusCode == 400)
			{
				return BadRequest("write something");
			}else if(res.StatusCode == 404)
			{
				return NotFound();
			}
			return BadRequest(res);

		}
	}
}