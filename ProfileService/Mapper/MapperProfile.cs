using AutoMapper;
using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Mapper
{
    public class MapperProfile : Profile
    {

       public MapperProfile()
        {
            CreateMap<CompleteLabourProfileDto, Labour>()
             .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.Ignore())
             .ForMember(dest => dest.LabourSkills, opt => opt.Ignore())
             .ForMember(dest => dest.LabourPreferedMuncipalities, opt => opt.Ignore())
             .ForMember(dest => dest.LabourWorkImages, opt => opt.Ignore()) 
             .ReverseMap();

            CreateMap<Labour, LabourViewDto>()
             .ForMember(dest => dest.LabourId, opt => opt.MapFrom(src => src.LabourId))
             .ForMember(dest => dest.LabourName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
             .ForMember(dest => dest.PreferedTime, opt => opt.MapFrom(src => src.PreferedTime))
             .ForMember(dest => dest.AboutYourSelf, opt => opt.MapFrom(src => src.AboutYourSelf))
             .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom(src => src.ProfilePhotoUrl))
             .ForMember(dest => dest.LabourWorkImages, opt => opt.MapFrom(src => src.LabourWorkImages.Select(img => img.ImageUrl).ToList()))
             .ForMember(dest => dest.LabourPreferredMuncipalities, opt => opt.MapFrom(src => src.LabourPreferedMuncipalities.Select(m => m.MunicipalityName).ToList()))
             .ForMember(dest => dest.LabourSkills, opt => opt.MapFrom(src => src.LabourSkills.Select(s => s.SkillName).ToList()))
             //.ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews
             //   .Select(r => new ReviewShowDto
             //   {
             //       Rating = r.Rating,
             //       Comment = r.Comment,
             //       Image = r.Image,
             //       FullName = r.Employer.FullName, 
             //       UpdatedAt = r.UpdatedAt
             //   }).ToList()))
             .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src =>
        (src.Reviews ?? new List<Review>())
            .Select(r => new ReviewShowDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                Image = r.Image,
                FullName = r.Employer.FullName, // Ensure Employer is not null or handle accordingly
                UpdatedAt = r.UpdatedAt
            }).ToList()))
             .ReverseMap();

            CreateMap<Employer, EmployerView>().ReverseMap();
            CreateMap<CompleteEmployerProfileDto, Employer>().ReverseMap();



        }

    }
}
