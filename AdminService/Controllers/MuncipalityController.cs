using AdminService.Dtos.MuncipalityDtos;
using AdminService.Models;
using AdminService.Services.MuncipalityService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuncipalityController : ControllerBase
    {
        private readonly IMuncipalityService _service;
        public MuncipalityController(IMuncipalityService service)
        {
            _service = service;
        }
        [HttpGet("muncipalities")]
        [Authorize]
        public async Task<IActionResult> GetAllMuncipalities()
        {

            try
            {
                var muncipalities = await _service.GetAll();
                if (muncipalities == null)
                {
                    Log.Warning("There is no muncipalities");
                    return NotFound();

                }
                Log.Information("Muncipalities fetched successfully");
                return Ok(muncipalities);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return StatusCode(400, ex.Message);
            }

        }
        [HttpPost("addmuncipality")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMuncipality(MuncipalityViewDto muncipality)
        {
            try
            {
                var res = await _service.AddMuncipality(muncipality);
                Log.Information("The muncipality Added Successfully");
                return Ok(res);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("mucipality{id}")]
        [Authorize]
        public async Task<IActionResult> GetMuncipality(int id)
        {
            try
            {
                var res = await _service.GetMuncipalityById(id);
                if (res == null)
                {
                    Log.Warning("There is no muncipality in this id");
                    return NotFound();
                }
                Log.Information($"muncipality in {id} is Listed");
                return Ok(res);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deletemuncipality{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMuncipality(int id)
        {
            var response = await _service.DeleteMuncipality(id);
            if (response.StatusCode == 204)
            {
                Log.Warning("There is no muncipality in this id");
                return NotFound();
            }
            Log.Information($"muncipality in {id} is Deleted");
            return Ok(response);
        }
        [HttpPatch("editmucipality")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMuncipality(MuncipalityViewDto muncipality)
        {
            try
            {

                var res = await _service.UpdateMuncipality(muncipality);
                if (res.StatusCode == 200)
                {
                    Log.Information("Muncipality Updated Successfully");
                    return Ok(res);
                }
                else
                {
                    Log.Information(res.Error);
                    return BadRequest(res);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("muncipalities{state}")]
        [Authorize]
        public async Task<IActionResult> GetMuncipalitiesByState(string state)
        {
            try
            {
                var result = await _service.GetMuncipalitiesByState(state);
                if (result.StatusCode == 204)
                {
                    Log.Warning("There is no muncipality in this state");
                    return NotFound();
                }
                Log.Information($"success muncipalities in the {state} is Listed ");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("viewallmuncipalities")]
        //[Authorize]
        public async Task<IActionResult> GetAllMuncipalitiesadmin()
        {

            try
            {
                var muncipalities = await _service.GetAllMuncipality();
                if (muncipalities == null)
                {
                    Log.Warning("There is no muncipalities");
                    return NotFound();

                }
                Log.Information("Muncipalities fetched successfully");
                return Ok(muncipalities);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return StatusCode(400, ex.Message);
            }

        }
        [HttpPatch("activatemuncipality/{id}")]
        [Authorize("Admin")]

        public async Task<IActionResult> ActivateMuncipality(int id)

        {
            try
            {
                var muncipalities = await _service.ActivateMuncipality(id);
                if (muncipalities == null)
                {
                    Log.Warning("There is no muncipalities");
                    return NotFound();

                }
                Log.Information("Muncipalities fetched successfully");
                return Ok(muncipalities);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return StatusCode(400, ex.Message);
            }

        }



        [HttpGet("searchMunciplities")]
        [Authorize]
        public async Task<IActionResult> SearchMunicpality(string searchkey)
        {
            try
            {
                var muncipalities = await _service.GetMuncipalityBySearchParams(searchkey);
                if (muncipalities.StatusCode==404)
                {
                    Log.Warning("There is no muncipalities");
                    return NotFound(muncipalities);

                }

                Log.Information("Muncipalities fetched successfully");
                return Ok(muncipalities);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return StatusCode(400, ex.Message);
            }
        }


    }

}
