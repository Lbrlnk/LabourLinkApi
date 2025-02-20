namespace ProfileService.Dtos
{
    public class LabourViewDto
    {
        public Guid LabourId { get; set; }
        public LabourProfileCompletionDto LabourProfileCompletion { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public List<string> LabourWorkImages { get; set; } = new List<string>();
        public List<int> LabourPreferredMuncipalities { get; set; } = new List<int>();
        public List<Guid> LabourSkills { get; set; } = new List<Guid>();
     }
}
