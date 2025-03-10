using AutoMapper;
using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;
using ProfileService.Models;
using ProfileService.Repositories.ChatConversationRepository;
using ProfileService.Repositories.EmployerRepository;
using ProfileService.Repositories.LabourRepository;

namespace ProfileService.Services.ConversationService
{
    public class ConversationService:IConversationService
    {

        private readonly IChatConversationRepository _chatConversationRepository;
        private readonly ILabourRepository _labourRepository;
        private readonly IEmployerRepository _employerRepository;

        private readonly IMapper _mapper;
        public ConversationService(IChatConversationRepository chatConversationRepository,IMapper mapper, ILabourRepository labourRepository, IEmployerRepository employerRepository)
        {

            _chatConversationRepository = chatConversationRepository;
            _labourRepository = labourRepository;
            _employerRepository = employerRepository;
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
                var res = await _chatConversationRepository.AddConversation(conversation);

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

        public async Task<ApiResponse<List<ConversationViewDto>>> GetEmployerConversation(Guid userId)
        {
            try
            {

                var checkuserType = await _employerRepository.GetEmployerByIdAsync(userId);
                var res = new List<ConversationViewDto>();
                if (checkuserType != null)
                {
                    var userConversation = await _chatConversationRepository.GetEmployerConversation(userId);

                    if (userConversation == null)
                    {


                        return new ApiResponse<List<ConversationViewDto>>(200, "Successfully retrived conversation", res);
                    }



                    //var result=_mapper.Map<List<ConversationDto>>(userConversation);
                    return new ApiResponse<List<ConversationViewDto>>(200, "Successfully retrived conversation", userConversation);

                }

                var checkIslabour = await _labourRepository.GetLabourByIdAsync(userId);

                if (checkIslabour != null)
                {
                    var result = await _chatConversationRepository.GetLabourConversation(userId);
                    if (res == null)
                    {
                        {


                            return new ApiResponse<List<ConversationViewDto>>(200, "Successfully retrived conversation", res);
                        }
                    }

                    return new ApiResponse<List<ConversationViewDto>>(200, "SuccessFully retrived conversation", result);

                }
                return new ApiResponse<List<ConversationViewDto>>(404, "No conversation found", res);

            }
            catch (Exception ex)
            {

                return new ApiResponse<List<ConversationViewDto>>(500, "Error ocure on creating conversation", error: ex.Message);

            }
        }

        public async Task<ApiResponse<ConversationDto>> UpdateLastMessage(Guid user1Id, Guid user2Id, string message)
        {

            try
            {
                var getConversation = await _chatConversationRepository.GetConversation(user1Id, user2Id);

                if (getConversation == null)
                {
                    var createConversation = await CreateChatConversation(user1Id, user2Id, message);

                }

                getConversation.LastMessage = message;
                getConversation.LastUpdated = DateTime.Now;

                var isUpdate = await _chatConversationRepository.UpdateConversation(getConversation);
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
