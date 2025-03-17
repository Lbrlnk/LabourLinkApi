namespace NotificationService.Dtos
{
    public class AcceptInterestDto
    {
        public Guid JobPostId { get; set; }
        public Guid InterestRequestId { get; set; }
        public Guid EmployerUserId { get; set; }
        public string EmployerName { get; set; }
        public Guid LabourUserId { get; set; }
        public string LabourName { get; set; }
        public string LabourImageUrl { get; set; }
    }
}
