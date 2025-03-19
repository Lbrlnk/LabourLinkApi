using ChatService.Dtos;
using ChatService.Model;

namespace ChatService.Services.ConversationServices
{
    public interface IConversationService
    {

        Task<ApiResponse<ConversationDto>> CreateChatConversation(Guid user1Id, Guid user2Id, string message);
        Task<ApiResponse<ConversationDto>> UpdateLastMessage(Guid user1Id, Guid user2Id, string message);
    }
}
