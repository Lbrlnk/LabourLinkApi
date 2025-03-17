using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Models
{
    public class LabourWorkImage
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid LabourId { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(500, ErrorMessage = "Image URL must not exceed 500 characters.")]
        public string? ImageUrl { get; set; }

        [ForeignKey("LabourId")]
        public Labour Labour { get; set; }
    }
}
