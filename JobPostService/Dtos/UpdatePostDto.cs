using System.ComponentModel.DataAnnotations;

namespace JobPostService.Dtos
{
	public class UpdatePostDto
	{
		[Required]
		[StringLength(40, ErrorMessage = "Skill name cannot exceed 40 characters")]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public decimal? Wage { get; set; }
		[Required]
		public DateTime? StartDate { get; set; }
		[Required]
		public string PrefferedTime { get; set; }
		[Required]
		public int? MuncipalityId { get; set; }
		[Required]
		public Guid? SkillId1 { get; set; }
		public Guid? SkillId2 { get; set; }
	}
}
