using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Services.LabourService;
using static System.Net.Mime.MediaTypeNames;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabourController : ControllerBase
    {

        private readonly ILabourService _labourService;

        public LabourController(ILabourService labourService)
        {
            _labourService = labourService;
        }




        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteLabourProfile([FromForm] CompleteLabourPeofileDto labourPeofileDto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());

            var labour = await _labourService.CompleteLabourProfile(labourPeofileDto, userId);

            return Ok(labour);
        }

        [HttpGet("all/lLabours")]
        public async Task<IActionResult> GetAllLabours()
        {
            try
            {
                var result = await _labourService.GetAllLabours();

                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);  
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        [HttpGet("filtered/labours")]
        public async Task<IActionResult> GetAllFilteredLabours(LabourFilterDto labourFilterDto)
        {
            try
            {
                if(labourFilterDto == null)
                {
                    return BadRequest("filter canot be null");
                }
                var result = _labourService.GetFilteredLabour(labourFilterDto);
                if(result == null)
                {
                    return NotFound("no labours found");
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        [HttpGet("getLabour")]
        public async Task<IActionResult>  GetUserById(Guid id)
        {
           var response = await _labourService.GetLabourById(id);

            if (response == null)
            {
                return NotFound("Labour not found");
            }
            return Ok(response);
        }

        [HttpGet("my-details")]
        public async Task<IActionResult> GetMyDetails()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }
            var UserId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var response = await _labourService.GetLabourById(UserId);

            if (response == null)
            {
                return NotFound("Labour not found");
            }
            return Ok(response);

        }

        [HttpDelete("delete/workimage")]
        public async Task<IActionResult> DeleTeLabourWorkImage(Guid WorkImage)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.DeleteLabourWorkImages(userId, WorkImage);
                if (result) return Ok("Image Deleted Successfully");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }
        [HttpDelete("delete/skill")]
        public async Task<IActionResult> DeleTeLabourSkill(Guid skillId)
        {

            try
            {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var result = await _labourService.DeleteLabourSkill(userId,skillId);
            if (result) return Ok("Skill Deleted Successfully");
            return BadRequest(result);
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }

        [HttpDelete("delete/municipality")]
        public async Task<IActionResult> DeleteLabourMunicipality(int municipalityId)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.DeleteLabourMunicipality(userId, municipalityId);
                if (result) return Ok("Muncipality Deleted Successfully");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }
        [HttpPost("add/municipality")]
        public async Task<IActionResult> AddLabourMunicipality(int municipalityId)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.AddLabourMunicipality(userId, municipalityId);
                if (result) return Ok("Muncipality added Successfully");
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        } 
        
        [HttpPost("add/skill")]
        public async Task<IActionResult> AddLabourSkill(Guid skillId)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.AddLabourSkill(userId, skillId);
                if (result) return Ok("Skill added Successfully");
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }
        
        [HttpPost("add/workimage")]
        public async Task<IActionResult> AddLabourSkill(IFormFile image)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.AddLabourWorkImage(userId, image);
                if (result) return Ok("WorkImage added Successfully");
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }

      
        [HttpPatch("edit/profile")]
        public async Task<IActionResult> EditLabourProfile([FromForm] EditLabourProfileDto editLabourProfileDto)
        {
            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.EditLabourProfile(userId, editLabourProfileDto);
                if (result) return Ok("your profile have updated");
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }
        }

    }
}
 