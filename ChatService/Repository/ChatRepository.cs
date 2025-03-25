using ChatService.Model;
using MongoDB.Driver;

namespace ChatService.Repository
{
    public class ChatRepository : IChatRepository
    {



        private readonly IMongoCollection<ChatMessage> _chatMessages;
        public ChatRepository(IMongoDatabase mongoDatabase)
        {
            _chatMessages = mongoDatabase.GetCollection<ChatMessage>("ChatMessages");
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(FilterDefinition<ChatMessage> filter)
        {

            //return (await _chatMessages.FindAsync(filter)).ToList();
            try
            {
                var result = await _chatMessages.FindAsync(filter);
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error retrieving chat history: {ex.Message}");
                return new List<ChatMessage>(); // Return empty list instead of crashing
            }
        }

        public async Task SaveChatMessage(ChatMessage chatMessage)
        {

            await _chatMessages.InsertOneAsync(chatMessage);

        }


    }
}