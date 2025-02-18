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
		public DateTime? StartDate { get; set; }
		public string PrefferedTime { get; set; }
		[Required]
		public int? MuncipalityId { get; set; }
		[Required]
		public string Muncipality {  get; set; }
		[Required]
		public Guid? SkillId1 { get; set; }
		[Required]
		public string Skill1 { get; set; }
		public Guid? SkillId2 { get; set; }

		public string Skill2 { get; set; }
		public string Status { get; set; } = "Active";
		[Required]
		public string Image { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

	}
}
