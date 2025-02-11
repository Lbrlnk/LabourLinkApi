using AuthenticationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos.AuthenticationDtos
{
    public class LabourRegistrationDto
    {
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
        ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string PasswordHash { get; set; }

        [Required]
        public UserType UserType { get; set; } = UserType.Labour;

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        public LabourPreferedTime PreferedTime { get; set; }    

        public int PreferedMuncipalityId { get; set; }


    }
}
