using NotificationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models
{
    public class InterestRequest
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid JobPostId { get; set; }  

        [Required]
        public Guid LabourerUserId { get; set; } 

        [Required]
        public Guid EmployerUserId { get; set; } 

        [Required]
        [EnumDataType(typeof(InterestRequestStatus))]
        public InterestRequestStatus Status { get; set; } = InterestRequestStatus.Pending;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
