using AuthenticationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

       
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

        [Required]
        public bool IsProfileCompleted { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
   



    }
}
