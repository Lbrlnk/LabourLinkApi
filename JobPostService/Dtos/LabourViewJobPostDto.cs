using System.ComponentModel.DataAnnotations;

namespace JobPostService.Dtos
{
	public class LabourViewJobPostDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Wage { get; set; }
		public DateOnly StartDate { get; set; }
		public string PrefferedTime { get; set; }
		public int MuncipalityId { get; set; }
		public string Status { get; set; }
		public Guid SkillId1 { get; set; }
		public Guid SkillId2 { get; set; }
		public string Image { get; set; }
		public DateOnly CreatedDate { get; set; }
	}
}
