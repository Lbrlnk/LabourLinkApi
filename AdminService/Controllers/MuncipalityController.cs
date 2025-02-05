using AdminService.Dtos;
using AdminService.Models;
using AdminService.Services.MuncipalityService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AdminService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MuncipalityController : ControllerBase
	{
		private readonly IMuncipalityService _service;
		private readonly IMapper _mapper;
		public MuncipalityController(IMuncipalityService service,IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}
		[HttpGet("All")]
		public async  Task<IActionResult> GetAllMuncipalities()
		{
			Log.Information("This is an info log message.");
			Log.Warning("This is a warning log message.");
			Log.Error("This is an error log message.");
			try
			{
				var muncipalities = await _service.GetAll();
				if(muncipalities == null)
				{
					return NotFound();
				}
				return Ok(muncipalities);
			}catch (Exception ex)
			{
				return StatusCode(400, ex.Message);
			}
			
		}
		[HttpPost]
		public async Task<IActionResult> AddMuncipality(MuncipalityViewDto muncipality)
		{
			try
			{
				var res=await _service.AddMuncipality(muncipality);
				return Ok(res);
			}catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("id")]
		public async Task<IActionResult> GetMuncipality(int id)
		{
			try
			{
				var res = await _service.GetMuncipalityById(id);
				if (res == null)
				{
					return NotFound();
				}
				return Ok(res);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteMuncipality(int id)
		{
			var response = await _service.DeleteMuncipality(id);
			return Ok(response);
		}
		[HttpPatch]
		public async Task<IActionResult> UpdateMuncipality(MuncipalityViewDto muncipality)
		{
			try
			{
				
				var res = await _service.UpdateMuncipality(muncipality);
				if (res.StatusCode == 200)
				{
					return Ok(res);
				}
				else
				{
					return BadRequest(res);
				}
			}catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("state")]
		public async Task<IActionResult> GetMuncipalitiesByState(string state)
		{
			try
			{
				var result = await _service.GetMuncipalitiesByState(state);
				if (result == null)
				{
					return NotFound();
				}
				return Ok(result);
			}catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
