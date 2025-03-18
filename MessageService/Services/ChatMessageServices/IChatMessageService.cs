using ChatService.Dtos;
using ChatService.Model;

namespace ChatService.Services.ChatService
{
    public interface IChatMessageService
    {

        Task SaveChatMessages(ChatMessage chatMessage);

        Task SaveYourChats(Guid userId, ChatDto chatDto);

        Task<List<ChatResponse>> GetChatHistoryAsync(Guid userId, Guid contactId, int limit = 50);
    }
}
