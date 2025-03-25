using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Dtos;
using NotificationService.Services.IntrestRequestService;
using System.Runtime.CompilerServices;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestRequestController : ControllerBase
    {
        private readonly IInterestRequestService _interestRequestService;
        public InterestRequestController(IInterestRequestService interestRequestService)
        {
            _interestRequestService = interestRequestService;
        }
        [HttpPost("show-interest")]
        public async Task<IActionResult> ShowInterest( [FromForm]InterestRequestDto intrst)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());


                var result = await _interestRequestService.AddInterestRequest(intrst,userId);
                if (result.StartsWith("Error:"))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawInterst(Guid id)
        {
            try
            {
                var result = await _interestRequestService.WithdrawInterstRequest(id);
                if (result.StartsWith("Error:"))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost("reject-request")]
        public async Task<IActionResult> RejectInterest(Guid id)
        {
            try
            {
                var result = await _interestRequestService.RejectInterestRequest(id);
                if (result.StartsWith("Error:"))
                
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}");
            }
        }
        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptRequest([FromForm] AcceptInterestDto acceptInterestDto)
        {
            


            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    return Unauthorized("User not authenticated.");
                }

                var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
                var result = await _interestRequestService.AcceptInterestRequest(acceptInterestDto, userId);
                if (result.StartsWith("Error:"))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}");
            }

        }

        [HttpGet("from-Labours")]
        public async Task<IActionResult> GetAllInterestRequest()
        {
            try
            {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }
            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var result = await _interestRequestService.GetInterestRequestForEmployers(userId);
            return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("get/accepted-interest-request")]
        public   async Task<IActionResult> GetAllInterestRequestLabour()
        {
            try
            {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }
            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
            var result = await _interestRequestService.GetAcceptedInterestRequestOfLabour(userId);
            return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }

    
}
