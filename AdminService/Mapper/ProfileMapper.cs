using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Mapper
{
	public class ProfileMapper:Profile
	{
		public ProfileMapper()
		{
			CreateMap<Muncipality,MuncipalityViewDto>().ReverseMap();
		}
	}
}
