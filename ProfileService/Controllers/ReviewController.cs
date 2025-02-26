using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Services.ReviewService;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _review;
        public ReviewController(IReviewService review)
		{
			_review = review;
		}
        [HttpPost("postreview")]
        public async Task<IActionResult> PostReview([FromForm] AddReview1 review1,IFormFile image)
        { 
			if (review1 == null)
			{
				return BadRequest("Please enter the details.");
			}
			if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}
			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var userId))
			{
				return BadRequest("Invalid UserId format.");
			}
			var res =await _review.AddReview(review1, image, userId);
            if (res.StatusCode == 200)
            {
                return Ok(res);
            }else 
            {
               return BadRequest(res);
            }
		}
		[HttpGet("getreviewsofspecificlabour")]
		public async Task<IActionResult> GetReviews(Guid Labourid)
		{
			var res = await _review.ShowReviews(Labourid);
			if (res.StatusCode == 200)
			{
				return Ok(res);
			}else if (res.StatusCode == 404)
			{
				return NotFound(res);
			}
			else
			{
				return BadRequest(res);
			}
		}
		[HttpPatch("Updatereview")]
		public async Task<IActionResult> UpdateReview(Guid reviewId, [FromForm] AddReview1 reviewDto, IFormFile? image)
		{
			if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}

			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var employerId))
			{
				return BadRequest("Invalid UserId format.");
			}
			var result = await _review.UpdateReview(reviewId, reviewDto, image, employerId);

			if (result.StatusCode == 200)
			{
				return Ok(result);
			}
			else if (result.StatusCode == 403)
			{
				return StatusCode(403, result);
			}
			else if (result.StatusCode == 404)  
			{
				return NotFound(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		[HttpPatch("updatethestatus")]
		public async Task<IActionResult> ChangeStatus(Guid reviewId)
		{
			if(!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] == null)
			{
				return BadRequest("UserId not found in the request context.");
			}

			var userIdString = HttpContext.Items["UserId"].ToString();
			if (!Guid.TryParse(userIdString, out var employerId))
			{
				return BadRequest("Invalid UserId format.");
			}
			var result = await _review.ChangeStatusAsync(reviewId, employerId);
			if (result.StatusCode == 200)
			{
				return Ok(result);
			}            
			else if (result.StatusCode == 403)
			{
				return StatusCode(403, result);
			}
			else if (result.StatusCode == 404)
			{
				return NotFound(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		[HttpGet("getreviewbyeachrating")]
		public async Task<IActionResult> GetNumberOfRating(Guid LabourId)
		{
			if (LabourId == null)
			{
				return BadRequest("Enter the LabourId");
			}
			var res = await _review.GetRating(LabourId);
			if (res.StatusCode == 200)
			{
				return Ok(res);
			}
			return BadRequest(res);

		}
	}
}


