using System.ComponentModel.DataAnnotations;

namespace JobPostService.Dtos
{
	public class LabourViewJobPostDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Wage { get; set; }
		public DateTime StartDate { get; set; }
		public string PrefferedTime { get; set; }
		public int MuncipalityId { get; set; }
		public string Status { get; set; }
		public string Muncipality { get; set; }
		[Required]
		public Guid? SkillId1 { get; set; }
		[Required]
		public string Skill1 { get; set; }
		public Guid? SkillId2 { get; set; }

		public string Skill2 { get; set; }
		public string Image { get; set; }
	}
}
