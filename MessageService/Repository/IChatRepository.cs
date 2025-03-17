using ChatService.Model;
using MongoDB.Driver;

namespace ChatService.Repository
{
    public interface IChatRepository
    {

        Task SaveChatMessage(ChatMessage chatMessage);

        Task<List<ChatMessage>> GetChatHistoryAsync(FilterDefinition<ChatMessage> filter);
    }
}
