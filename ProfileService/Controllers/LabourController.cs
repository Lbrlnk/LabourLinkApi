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




        [HttpPost("complete/profile")]
        public async Task<IActionResult> CompleteLabourProfile([FromForm] CompleteLabourProfileDto labourPeofileDto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());

            var labour = await _labourService.CompleteLabourProfile(labourPeofileDto, userId);

            return Ok(labour);
        }

        [HttpGet("labours")]
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

        [HttpPost("filter/labours")]
        public async Task<IActionResult> GetAllFilteredLabours([FromForm] LabourFilterDto labourFilterDto)
        {
            try
            {
                if(labourFilterDto == null)
                {
                    return BadRequest("filter canot be null");
                }
                var result = await _labourService.GetFilteredLabour(labourFilterDto);
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

        [HttpPost("labour")]
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
            var response = await _labourService.GetMyDetails(UserId);

            if (response == null)
            {
                return NotFound("Labour not found");
            }
            return Ok(response);

        }

        [HttpDelete("add/workimage")]
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
        public async Task<IActionResult> DeleTeLabourSkill(string skillName)
        {

            try
            {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var result = await _labourService.DeleteLabourSkill(userId,skillName);
            if (result) return Ok("Skill Deleted Successfully");
            return BadRequest(result);
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }

        [HttpDelete("add/municipality")]
        public async Task<IActionResult> DeleteLabourMunicipality(string municipalityName)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.DeleteLabourMunicipality(userId, municipalityName);
                if (result) return Ok("Muncipality Deleted Successfully");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        }
        [HttpPost("add/municipality")]
        public async Task<IActionResult> AddLabourMunicipality(string municipalityName)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.AddLabourMunicipality(userId, municipalityName);
                if (result) return Ok("Muncipality added Successfully");
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });

            }


        } 
        
        [HttpPost("aad/skill")]
        public async Task<IActionResult> AddLabourSkill(string  skillName)
        {

            try
            {

                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _labourService.AddLabourSkill(userId, skillName);
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
 