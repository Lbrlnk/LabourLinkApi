namespace ProfileService.Dtos
{
	public class SkillAnalyticsDto
	{
		public Dictionary<string, int> MainSkills { get; set; } = new();
		public int OtherSkillsCount { get; set; }
	}
}
