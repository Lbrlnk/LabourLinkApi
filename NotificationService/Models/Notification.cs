using NotificationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models
{
    public class Notification
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid JobPostId { get; set; }
        [Required]
        public Guid SenderId { get; set; }
        [Required] 
        
        public Guid ReceicverId { get; set; }
        [EnumDataType(typeof(NotificationType))]
        public NotificationType notificationType { get; set; }
        [EnumDataType(typeof(InterestRequestStatus))]
        public InterestRequestStatus Status { get; set; } 
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
