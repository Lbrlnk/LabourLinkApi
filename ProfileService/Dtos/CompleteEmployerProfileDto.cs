using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class CompleteEmployerProfileDto
    {
        [Required]
   
        public string FullName { get; set; }

        public IFormFile ProfileImage { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        public string PreferedMunicipality { get; set; }

    }
}
