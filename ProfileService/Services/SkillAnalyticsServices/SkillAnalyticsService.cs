using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dtos;

namespace ProfileService.Services.SkillAnalyticsServices
{
	public class SkillAnalyticsService:ISkillAnalyticsService
	{
		private readonly LabourLinkProfileDbContext _context;

		public SkillAnalyticsService(LabourLinkProfileDbContext context)
		{
			_context = context;
		}

		public async Task<SkillAnalyticsDto> GetSkillAnalyticsAsync()
		{
			var skillCounts = await _context.LabourSkills
				.GroupBy(ls => ls.SkillName)
				.Select(g => new { SkillName = g.Key, Count = g.Count() })
				.ToListAsync();

			int totalSkills = skillCounts.Sum(s => s.Count);  
			int threshold = (int)Math.Ceiling(totalSkills * 0.10);  

			var mainSkills = skillCounts
				.Where(s => s.Count >= threshold)
				.ToDictionary(s => s.SkillName, s => s.Count);

			int othersCount = skillCounts
				.Where(s => !mainSkills.ContainsKey(s.SkillName))
				.Sum(s => s.Count);

			return new SkillAnalyticsDto
			{
				MainSkills = mainSkills,
				OtherSkillsCount = othersCount
			};
		}
		public async Task<List<MunicipalityCountDto>> GetMunicipalityPreferencesAsync()
		{
			var result = await _context.LabourPreferedMuncipalities
				.GroupBy(lpm => lpm.MunicipalityName)
				.Select(group => new MunicipalityCountDto
				{
					MunicipalityName = group.Key,
					LabourCount = group.Count()
				})
				.OrderByDescending(x => x.LabourCount) 
				.ToListAsync();

			return result;
		}
	}

}
