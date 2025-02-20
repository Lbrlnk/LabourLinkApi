using ProfileService.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class EditLabourProfileDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string? FullName { get; set; }
       
        [Required]
        public LabourPreferedTime? PreferedTime { get; set; }

        [MaxLength(500, ErrorMessage = "About Yourself must be at most 500 characters.")]
        public string? AboutYourSelf { get; set; }
        public IFormFile? Image { get; set; }
    }
}
