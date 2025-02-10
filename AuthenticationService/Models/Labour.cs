using AuthenticationService.Enums;

namespace AuthenticationService.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Labour
    {
        [Key]
        public Guid LabourId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public LabourPreferedTime PreferedTime { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public int Rating { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; }
    }

}
