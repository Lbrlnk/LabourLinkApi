using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Services.SkillAnalyticsServices;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
		private readonly ISkillAnalyticsService _analyticsService;

		public AnalyticsController(ISkillAnalyticsService analyticsService)
		{
			_analyticsService = analyticsService;
		}

		[HttpGet("skillsAnalytics")]
		public async Task<IActionResult> GetSkillAnalytics()
		{
			var result = await _analyticsService.GetSkillAnalyticsAsync();
			return Ok(result);
		}
		[HttpGet("municipalityanalytics")]
		public async Task<ActionResult> GetMunicipalityPreferences()
		{
			var result = await _analyticsService.GetMunicipalityPreferencesAsync();
			return Ok(result);
		}
	}
}
