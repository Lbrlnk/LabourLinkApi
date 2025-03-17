using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Repositories.ChatConversationRepository
{
    public interface IChatConversationRepository
    {
        Task<Conversation> AddConversation(Conversation conversation);

        Task<Conversation> GetConversation(Guid user1Id, Guid user2Id);
        Task<bool> UpdateConversation(Conversation conversation);

        Task<List<ConversationViewDto>> GetEmployerConversation(Guid userId);

        Task<List<ConversationViewDto>> GetLabourConversation(Guid userId);
    }
}
