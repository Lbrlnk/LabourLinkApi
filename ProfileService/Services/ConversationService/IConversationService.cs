using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;

namespace ProfileService.Services.ConversationService
{
    public interface IConversationService
    {

        Task<ApiResponse<ConversationDto>> CreateChatConversation(Guid user1Id, Guid user2Id, string message);
        Task<ApiResponse<ConversationDto>> UpdateLastMessage(Guid user1Id, Guid user2Id, string message);

        Task<ApiResponse<List<ConversationViewDto>>> GetEmployerConversation(Guid userId);
    }
}
