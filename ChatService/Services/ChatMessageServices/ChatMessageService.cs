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
               
                var chatres = await _chatRepository.GetChatHistoryAsync( userId,  contactId,  limit = 50);
                var chathistory = chatres.OrderBy(x => x.Timestamp)
                    .Take(limit)
                    .Select(x => new ChatResponse
                    {
                        MessageId = x.ChatMessageId,
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
                await _chatRepository.SaveChatMessageAsync(chatMessage);
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

                await _chatRepository.SaveChatMessageAsync(res);


            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while Saving Chat Message, error: {ex.Message}");
            }



        }

    }
}
