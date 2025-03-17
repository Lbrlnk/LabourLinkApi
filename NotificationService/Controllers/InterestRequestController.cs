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
        public async Task<IActionResult> ShowInterest(InterestRequestDto intrst)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result =  await _interestRequestService.AddInterestRequest(intrst);
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
               var result =  await _interestRequestService.WithdrawInterstRequest(id);
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
        public async Task<IActionResult> AcceptRequest(AcceptInterestDto acceptInterestDto)
        {
            try
            {
                var result = await _interestRequestService.AcceptInterestRequest(acceptInterestDto);
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

    }
}
