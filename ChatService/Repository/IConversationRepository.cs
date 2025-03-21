using ChatService.Model;

namespace ChatService.Repository
{
    public interface IConversationRepository
    {
        Task<Conversation> AddConversation(Conversation conversation);

        Task<Conversation> GetConversation(Guid user1Id, Guid user2Id);
        Task<bool> UpdateConversation(Conversation conversation);
    }
}
