using ProfileService.Dtos;

namespace ProfileService.Services.SkillAnalyticsServices
{
	public interface ISkillAnalyticsService
	{
		Task<SkillAnalyticsDto> GetSkillAnalyticsAsync();
		Task<List<MunicipalityCountDto>> GetMunicipalityPreferencesAsync();
	}
}
