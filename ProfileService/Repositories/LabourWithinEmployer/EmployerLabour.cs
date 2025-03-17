using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;

namespace ProfileService.Repositories.LabourWithinEmployer
{
	public class EmployerLabour : IEmployerLabour
	{
		private readonly LabourLinkProfileDbContext _context;
		private readonly IMapper _mapper;

		public EmployerLabour(LabourLinkProfileDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<LabourViewDto>>> GetLabourByEmployerMun(Guid userid, int pageNumber, int pageSize)
		{
			var employermun = await _context.Employers
									.Where(x => x.UserId == userid)
									.Select(x => x.PreferedMunicipality)
									.ToListAsync();

			if (!employermun.Any())
			{
				return new ApiResponse<List<LabourViewDto>>(404, "Employer municipality not found", null);
			}

			var result = await _context.Labours
				.Include(x => x.LabourPreferedMuncipalities)
				.Include(x => x.LabourSkills)
				.Include(x => x.LabourWorkImages)
				.Where(x => x.LabourPreferedMuncipalities
								.Any(m => employermun.Contains(m.MunicipalityName)))
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			if (!result.Any())
			{
				return new ApiResponse<List<LabourViewDto>>(404, "No matching labours found", null);
			}

			var labourViewDtos = result.Select(labour => new LabourViewDto
			{
				LabourId = labour.LabourId,
				LabourName = labour.FullName,
				PhoneNumber = labour.PhoneNumber,
				Rating = labour.Rating,
				PreferedTime = labour.PreferedTime,
				AboutYourSelf = labour.AboutYourSelf,
				ProfilePhotoUrl = labour.ProfilePhotoUrl,
				LabourWorkImages = labour.LabourWorkImages.Select(img => img.ImageUrl).ToList(),
				LabourPreferredMuncipalities = labour.LabourPreferedMuncipalities.Select(m => m.MunicipalityName).ToList(),
				LabourSkills = labour.LabourSkills.Select(s => s.SkillName).ToList()
			}).ToList();

			return new ApiResponse<List<LabourViewDto>>(200, "Success", labourViewDtos);
		}

	}
}
