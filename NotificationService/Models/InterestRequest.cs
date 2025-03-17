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
        public Guid LabourUserId { get; set; } 
        [Required]

        public string LabourName { get; set; }

        public Guid EmployerUserId { get; set; }

        public string EmployerName { get; set; }
 

        [Required]
        [EnumDataType(typeof(InterestRequestStatus))]
        public InterestRequestStatus Status { get; set; } = InterestRequestStatus.Pending;

        public bool WithDrawRequest { get; set; } = false;

        public bool IsDelete { get; set; } = false;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; }= DateTime.UtcNow;
    }
}
