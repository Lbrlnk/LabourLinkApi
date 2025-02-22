using AuthenticationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos.AuthenticationDtos
{
    public class RegistrationDto
    {

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
       ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }
    }
}
