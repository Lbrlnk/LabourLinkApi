using ChatService.Model;
using MongoDB.Driver;

namespace ChatService.Repository
{
    public interface IChatRepository
    {

        Task SaveChatMessageAsync(ChatMessage chatMessage);

        Task<List<ChatMessage>> GetChatHistoryAsync(Guid userId, Guid contactId, int limit = 50);
    }
}
