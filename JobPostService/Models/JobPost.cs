using System.ComponentModel.DataAnnotations;

namespace JobPostService.Models
{
	public class JobPost
	{
		[Key]
		public Guid JobId { get; set; }
		[Required]
		public Guid CleintId { get; set; }
		[Required]
		[StringLength(40, ErrorMessage = "Skill name cannot exceed 40 characters")]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		public decimal? Wage { get; set; }
		public DateOnly? StartDate { get; set; }
		[Required]
		public DateOnly EndDate { get; set; }
		public string PrefferedTime { get; set; }
		[Required]
		public string MuncipalityId { get; set; }
		[Required]
		public string SkillId1 { get; set; }
		public string SkillId2 { get; set; }

		public string Status { get; set; } = "Active";
		[Required]
		public string Image { get; set; }
		public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
		public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;



	}
}
