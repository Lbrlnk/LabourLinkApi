using AuthenticationService.Dtos.AuthenticationDtos;
using AuthenticationService.Models;
using AutoMapper;

namespace AuthenticationService.Mapper
{
    public class MapperProfile : Profile
    {
       public  MapperProfile()
        {
            
            CreateMap<User, LoginDto>().ReverseMap();
        }
    }
}
