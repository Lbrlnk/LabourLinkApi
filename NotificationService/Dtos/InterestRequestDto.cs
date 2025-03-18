using NotificationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Dtos
{
    public class InterestRequestDto
    {
 
        public Guid JobPostId { get; set; }
        public Guid LabourUserId { get; set; }
        public string LabourName { get; set; }
        public string LabourImageUrl { get; set; }
        public Guid EmployerUserId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerImageUrl { get; set; }

    }
}
