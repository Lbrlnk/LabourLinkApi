using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
	public class Review
	{
		[Key]
		public Guid ReviewId { get; set; }
		public Guid EmployerId { get; set; }
		[Required]
		public Guid LabourId { get; set; }
		[Required]
		[Range(1, 5, ErrorMessage = "The rating must be between 1 and 5.")]
		public decimal Rating { get; set; }
		public string? Comment { get; set; }
		public string Image { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public bool IsActive { get; set; } = false;
		public virtual Labour Labour { get; set; }
		public virtual  Employer Employer { get; set; }
		
	}
}
