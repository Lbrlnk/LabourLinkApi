using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
	public class AddReviewDto
	{
		public Guid EmployerId { get; set; }
		public Guid LabourId { get; set; }
		[Required]
		[Range(1, 5, ErrorMessage = "The rating must be between 1 and 5.")]
		public decimal Rating { get; set; }
		public string? Comment { get; set; }
	}
}
