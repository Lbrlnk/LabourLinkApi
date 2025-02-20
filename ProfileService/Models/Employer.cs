using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
    public class Employer
    {
        public Guid EmployerId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        public int PreferedMuncipalityId { get; set; }



        
    }
}
