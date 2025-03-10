using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class LabourFilterDto
    {
       
            public List<string>? PreferredMunicipalities { get; set; } = new List<string>();
            public List<string>? Skills { get; set; } = new List<string>();

        
    }
}
