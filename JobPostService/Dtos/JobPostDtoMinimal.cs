namespace JobPostService.Dtos
{
	public class JobPostDtoMinimal
	{

        public Guid JobId { get; set; }
        public Guid CleintId { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
		public decimal? Wage { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public string PrefferedTime { get; set; }
		public string MuncipalityId { get; set; }
		public string Status { get; set; }
		public string SkillId1 { get; set; }
		public string SkillId2 { get; set; }
		public string Image { get; set; }
		public DateOnly CreatedDate { get; set; }
		public string FullName { get; set; }
		public string? ProfileImageUrl { get; set; }
	}
}
