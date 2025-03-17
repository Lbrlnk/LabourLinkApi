using ChatService.Data;
using ChatService.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Repository
{
    public class ConversationRepository : IConversationRepository
    {

        private readonly ChatDbContext _context;
        public ConversationRepository(ChatDbContext chatDbContext)
        {
            _context = chatDbContext;
        }
        public async Task<Conversation> AddConversation(Conversation conversation)
        {
            var checkALreadyExit = await _context.Conversations.
                FirstOrDefaultAsync(x =>
                x.User1Id == conversation.User1Id && x.User2Id == conversation.User2Id ||
                x.User2Id == conversation.User1Id && x.User1Id == conversation.User2Id);

            if (checkALreadyExit != null)
            {
                return null;
            }
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Conversation> GetConversation(Guid user1Id, Guid user2Id)
        {
            var checkConversation = await _context.Conversations.
               FirstOrDefaultAsync(x =>
               x.User1Id == user1Id && x.User2Id == user2Id ||
               x.User2Id == user1Id && x.User1Id == user2Id);

            if (checkConversation == null)
            {
                return null;
            }
            return checkConversation;


        }

        public async Task<bool> UpdateConversation(Conversation conversation)
        {

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
