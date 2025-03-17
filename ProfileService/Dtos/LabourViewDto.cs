using ProfileService.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class LabourViewDto
    {
        public Guid LabourId { get; set; }
        public string LabourName { get; set; }
        public string PhoneNumber { get; set; }    
        public LabourPreferedTime PreferedTime { get; set; }
        public string? AboutYourSelf { get; set; }

        public decimal Rating { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public List<string> LabourWorkImages { get; set; } = new List<string>();
        public List<string> LabourPreferredMuncipalities { get; set; } = new List<string>();
        public List<string> LabourSkills { get; set; } = new List<string>();
        public List<ReviewShowDto> Reviews { get; set; } = new List<ReviewShowDto>();


    }
}
