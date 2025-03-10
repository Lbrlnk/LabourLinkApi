using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class LabourFilterDto
    {
       
            public List<int>? PreferredMunicipalities { get; set; } = new List<int>();
            public List<Guid>? SkillIds { get; set; } = new List<Guid>();

        
    }
}
