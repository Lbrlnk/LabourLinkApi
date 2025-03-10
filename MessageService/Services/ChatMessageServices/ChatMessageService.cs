using AutoMapper;
using ChatService.Dtos;
using ChatService.Model;
using ChatService.Repository;
using MongoDB.Driver;

namespace ChatService.Services.ChatService
{
    public class ChatMessageService:IChatMessageService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;
        public ChatMessageService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<List<ChatResponse>> GetChatHistoryAsync(Guid userId, Guid contactId, int limit = 50)
        {
            try
            {
                var filter = Builders<ChatMessage>.Filter.Or(
                    Builders<ChatMessage>.Filter.And(
                    Builders<ChatMessage>.Filter.Eq(x => x.SenderId, userId),
                    Builders<ChatMessage>.Filter.Eq(x => x.ReceiverId, contactId)
                    ),
                    Builders<ChatMessage>.Filter.And(
                    Builders<ChatMessage>.Filter.Eq(x => x.SenderId, contactId),
                    Builders<ChatMessage>.Filter.Eq(x => x.ReceiverId, userId)
                    )
                    );
                var chatres = await _chatRepository.GetChatHistoryAsync(filter);
                var chathistory = chatres.OrderBy(x => x.Timestamp)
                    .Take(limit)
                    .Select(x => new ChatResponse
                    {
                        MessageId = x.MessageId,
                        SenderId = x.SenderId,
                        ReceiverId = x.ReceiverId,
                        Message = x.Message,
                        Timestamp = x.Timestamp,
                        IsYouSender = x.SenderId == userId,

                    })
                    .ToList();
                return chathistory;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving  Chat History, error: {ex.Message}");
            }

        }

        public async Task SaveChatMessages(ChatMessage chatMessage)
        {
            try
            {
                await _chatRepository.SaveChatMessage(chatMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while Saving Chat Message, error: {ex.Message}");
            }
        }

        public async Task SaveYourChats(Guid userId, ChatDto chatDto)
        {

            try
            {
                var res = _mapper.Map<ChatMessage>(chatDto);
                res.SenderId = userId;

                await _chatRepository.SaveChatMessage(res);


            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while Saving Chat Message, error: {ex.Message}");
            }



        }

    }
}
