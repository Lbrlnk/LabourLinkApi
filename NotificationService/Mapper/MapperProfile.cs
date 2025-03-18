using AutoMapper;
using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<InterestRequest, InterestRequestDto>().ReverseMap();
            CreateMap<Notification , NotificationViewDto>().ReverseMap();
            //CreateMap<Notification , AcceptInterestDto>
        }
    }
}
