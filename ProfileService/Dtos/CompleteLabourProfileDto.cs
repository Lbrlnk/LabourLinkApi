using ProfileService.Enums;
using ProfileService.Models;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class CompleteLabourProfileDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public LabourPreferedTime PreferedTime { get; set; }

        [MaxLength(500, ErrorMessage = "About Yourself must be at most 500 characters.")]
        public string? AboutYourSelf { get; set; }

        public IFormFile? ProfileImage { get; set; }

  

        public List<string> LabourPreferredMunicipalities { get; set; } = new List<string>();

        private List<IFormFile?> _labourWorkImages = new List<IFormFile?>();
        public List<IFormFile?> LabourWorkImages 
        {
            get => _labourWorkImages;
            set
            {
                if (value != null && value.Count > 3)
                {
                    throw new ArgumentException("Maximum of 3 work images allowed.", nameof(LabourWorkImages));
                }
                _labourWorkImages = value;
            }
        }
        public List<string> LabourSkills { get; set; } = new List<string>();



    }
}
