using NotificationService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Dtos
{
    public class NotificationViewDto
    {  

        public Guid Id { get; set; } 
        public Guid JobPostId { get; set; }
        public Guid SenderUserId { get; set; }
        public string SednderImageUrl { get; set; }
        public string SenderName { get; set; } 
        public string ReceiverUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType notificationType { get; set; }
        public DateTime CreatedOn { get; set; } 
    }
}
