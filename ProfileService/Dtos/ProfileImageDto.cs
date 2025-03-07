using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class ProfileImageDto
    {
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
