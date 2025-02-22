using ProfileService.Enums;
using ProfileService.Models;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class LabourProfileCompletionDto
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

    }



    //public class ValidateWorkImages : ValidationAttribute
    //{
    //    public override bool IsValid(object? value)
    //    {
    //        if (value is List<LabourWorkImageDto> images)
    //            return images.Count <= 3;
    //        return true;
    //    }
    //}
}
