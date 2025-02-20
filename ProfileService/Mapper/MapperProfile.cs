using AutoMapper;
using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Mapper
{
    public class MapperProfile : Profile
    {

       public MapperProfile()
        {
            CreateMap<LabourProfileCompletionDto, Labour>().ReverseMap();
            //CreateMap<LabourMuncipalitiesDto, LabourPreferedMuncipality>().ReverseMap();
            //CreateMap<LabourSkillDto, LabourSkills>().ReverseMap();
            //CreateMap<LabourMuncipalitiesDto, LabourPreferredMuncipality>()
            //.ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.MunicipalityId));
            CreateMap<CompleteEmployerProfileDto , Employer>().ReverseMap();

        }

    }
}
