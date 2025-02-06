using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class Muncipality
    {

        [Key]
        public int MunicipalityId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? State { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
