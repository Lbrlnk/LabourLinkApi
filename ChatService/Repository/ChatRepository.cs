using ChatService.Data;
using ChatService.Model;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Repository
{
    public class ChatRepository : IChatRepository
    {



        //private readonly IMongoCollection<ChatMessage> _chatMessages;
        //public ChatRepository(IMongoDatabase mongoDatabase)
        //{
        //    _chatMessages = mongoDatabase.GetCollection<ChatMessage>("ChatMessages");
        //}

        //public async Task<List<ChatMessage>> GetChatHistoryAsync(FilterDefinition<ChatMessage> filter)
        //{

        //    return (await _chatMessages.FindAsync(filter)).ToList();
        //}

        //public async Task SaveChatMessage(ChatMessage chatMessage)
        //{

        //    await _chatMessages.InsertOneAsync(chatMessage);

        //}

        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(Guid userId, Guid contactId, int limit = 50)
        {
            return await _context.ChatMessages
                .Where(x =>
                    (x.SenderId == userId && x.ReceiverId == contactId) ||
                    (x.SenderId == contactId && x.ReceiverId == userId))
                .OrderBy(x => x.Timestamp)
                .Take(limit)
                .ToListAsync();
        }

        public async Task SaveChatMessageAsync(ChatMessage chatMessage)
        {
            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();
        }


    }
}