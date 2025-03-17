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
        public Guid SenderUserId { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string SenderImageUrl { get; set; }
        [Required]
        public Guid ReceiverUserId { get; set; }
        [Required]
        public string ReceicverName { get; set; }
        public string Message { get; set; }
        [EnumDataType(typeof(NotificationType))]
        public NotificationType NotificationType { get; set; }
       
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
