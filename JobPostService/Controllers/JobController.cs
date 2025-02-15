using JobPostService.Dtos;
using JobPostService.Helpers.ApiResonse;
using JobPostService.Services;
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
		[HttpPost("createjobpost")]
		public async Task<IActionResult> PostAJob([FromForm] JobPostDto jobPostDto,IFormFile image)
		{

			var response= await _service.AddNewPost(jobPostDto, image);
			if (response.StatusCode == 200)
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
		[HttpPatch("updatejobpost")]
		public async Task<IActionResult> UpdateJobPost(Guid jobId, Guid clientId,UpdatePostDto updatePostDto)
		{
			if (updatePostDto == null)
			{
				return BadRequest(new ApiResponse<string>(400, "Invalid request data."));
			}
			var res = await _service.UpdateJobPost(updatePostDto, clientId, jobId);
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
		[HttpPatch("changestatus")]
		public async Task<IActionResult> UpdateStatus(string status, Guid jobid, Guid clientid)
		{
			var res=await _service.ChangeStatus(status, jobid, clientid);
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
		[HttpGet("jobpostbyclient")]
		public async Task<IActionResult> ShowJobPostByClient(Guid cleintid)
		{
			var res=await _service.GetJobPostByClientid(cleintid);
			if(res.StatusCode == 200)
			{
				return Ok(res);
			}else if (res.StatusCode == 404)
			{
				return NoContent();
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
		[HttpGet("filterjobpostwithskill")]
		public async Task<IActionResult> Filterjobpostskill(Guid skillid)
		{
			if (skillid == null)
			{
				return BadRequest("Enter the skill");
			}
			var result=await _service.FilterJopPostBasedOnSkill(skillid);
			if(result.StatusCode == 200)
			{
				return Ok(result);
			}else if(result.StatusCode ==404) {
				return NotFound(result);
			}
			return BadRequest(result);
		}
		[HttpGet("filterjobpostwithmuncipality")]
		public async Task<IActionResult> Filterjobpostmuncipality(int muncipalityid)
		{
			if (muncipalityid == null)
			{
				return BadRequest("Enter the muncipality");
			}
			var result = await _service.FilterJopPostBasedOnMuncipality(muncipalityid);
			if (result.StatusCode == 200)
			{
				return Ok(result);
			}
			else if (result.StatusCode == 404)
			{
				return NotFound(result);
			}
			return BadRequest(result);
		}
	}
}