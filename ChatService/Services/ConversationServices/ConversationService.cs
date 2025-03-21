using AutoMapper;
using ChatService.Dtos;
using ChatService.Model;
using ChatService.Repository;

namespace ChatService.Services.ConversationServices
{
    public class ConversationService : IConversationService
    {

        private readonly IConversationRepository _conversationRepository;
        private readonly IMapper _mapper;
        public ConversationService(IConversationRepository conversationRepository, IMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ConversationDto>> CreateChatConversation(Guid user1Id, Guid user2Id, string message)
        {
            try
            {
                var data = new Conversation
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    LastMessage = message

                };


                var conversation = _mapper.Map<Conversation>(data);
                var res = await _conversationRepository.AddConversation(conversation);

                if (res == null)
                {
                    return new ApiResponse<ConversationDto>(200, "No need of Conversation required , already exist", null);
                }

                var result = _mapper.Map<ConversationDto>(res);

                return new ApiResponse<ConversationDto>(201, "Successfully Conversation ", result);
            }
            catch (Exception ex)
            {

                return new ApiResponse<ConversationDto>(500, "Error ocure on creating conversation", error: ex.Message);

            }
        }

        public async Task<ApiResponse<ConversationDto>> UpdateLastMessage(Guid user1Id, Guid user2Id, string message)
        {

            try
            {
                var getConversation = await _conversationRepository.GetConversation(user1Id, user2Id);

                if (getConversation == null)
                {
                    var createConversation = await CreateChatConversation(user1Id, user2Id, message);

                }

                getConversation.LastMessage = message;
                getConversation.LastUpdated = DateTime.Now;

                var isUpdate = await _conversationRepository.UpdateConversation(getConversation);
                var res = _mapper.Map<ConversationDto>(getConversation);

                if (isUpdate)
                {

                    return new ApiResponse<ConversationDto>(200, "Successfully Updated Last Message", res);
                }
                return new ApiResponse<ConversationDto>(400, "Updating last message failed", res);


            }
            catch (Exception ex)
            {

                return new ApiResponse<ConversationDto>(500, "Error ocure on update last message conversation", error: ex.Message);
            }
        }


    }
}
