using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
    public class LabourSkills
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "LabourId is required.")]
        public Guid LabourId { get; set; }

        [Required(ErrorMessage = "SkillId is required.")]
        public string SkillName { get; set; }

        [ForeignKey("LabourId")]
        public Labour Labour { get; set; }
    }
}
