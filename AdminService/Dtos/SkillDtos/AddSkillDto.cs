using System.ComponentModel.DataAnnotations;

namespace AdminService.Dtos.SkillDtos
{
    public class AddSkillDto
    {


        [Required]
        [StringLength(40, ErrorMessage = "Skill name cannot exceed 40 characters")]
        public string Name { get; set; }
    }

}