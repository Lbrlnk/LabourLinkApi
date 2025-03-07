using ProfileService.Models;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class CompleteLabourPeofileDto
    {
        public LabourProfileCompletionDto LabourProfileCompletionDto { get; set; }
        public ProfileImageDto? ProfileImageDto { get; set; }

  

        public List<int> LabourPreferredMunicipalities { get; set; } = new List<int>();

        private List<IFormFile?> _labourWorkImages = new List<IFormFile?>();
        public List<IFormFile?> LabourWorkImages 
        {
            get => _labourWorkImages;
            set
            {
                if (value != null && value.Count > 3)
                {
                    throw new ArgumentException("Maximum of 3 work images allowed.", nameof(LabourWorkImages));
                }
                _labourWorkImages = value;
            }
        }
        public List<Guid> LabourSkills { get; set; } = new List<Guid>();



    }
}
