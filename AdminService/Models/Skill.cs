using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class Skill
    {

        [Key]
        public Guid SkillId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Skill name cannot exceed 40 characters")]
        public string SkillName { get; set; }

        public bool IsActive { get; set; }=true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


    }
}
