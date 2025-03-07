using AutoMapper;
using JobPostService.Dtos;
using JobPostService.Models;

namespace JobPostService.Mapper
{
	public class ProfileMapper:Profile
	{
		public ProfileMapper()
		{
			CreateMap<JobPost, LabourViewJobPostDto>().ReverseMap();
		}
	}
}
