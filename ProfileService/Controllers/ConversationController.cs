using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Repositories.ChatConversationRepository;
using ProfileService.Services.ConversationService;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("create/converasation")]

        public async Task<IActionResult> CreateConversation(Guid user2Id, string message)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
            {
                return Unauthorized("User not authenticated.");
            }
            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());

            var response = await _conversationService.CreateChatConversation(userId, user2Id, message);

            return Ok(response);
        }

        [HttpGet("Getuserconversation")]
        public async Task<IActionResult> GetUserConversation()
        {

            if (!HttpContext.Items.ContainsKey("UserId"))
            {
               
                return Unauthorized("User not authenticated.");
            }
            var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());


            var response = await _conversationService.GetEmployerConversation(userId);
            return Ok(response);

        }
    }
}
