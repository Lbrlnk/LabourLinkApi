using AdminService.Dtos.SkillDtos;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Skill, SkillViewDto>().ReverseMap();
            CreateMap<Skill, AdminViewSkillDto>();

        }

    }
}
