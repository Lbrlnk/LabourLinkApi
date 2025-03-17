using AutoMapper;
using ChatService.Dtos;
using ChatService.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ChatService.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {

            CreateMap<ChatMessage, ChatDto>().ReverseMap();
            CreateMap<ConversationDto, Conversation>().ReverseMap();
        }
    }
}
