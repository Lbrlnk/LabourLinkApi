using ChatService.Services.ChatService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly IChatMessageService _chatMessageService;

        public ChatController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }


        [HttpGet("/chatmessage/history")]

        public async Task<IActionResult> GetChatHistory(string reciverId)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());

            var res = await _chatMessageService.GetChatHistoryAsync(userId, Guid.Parse(reciverId));
            return Ok(res);
        }

    }
}
