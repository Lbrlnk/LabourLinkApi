using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class ConversationViewDto
    {

        public Guid Id { get; set; }
        [Required]

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string? ProfilePhotoUrl { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public bool IsBlocked { get; set; } = false;  // True if chat is blocked
        public Guid? BlockedByUserId { get; set; }  // Stores the user who blocked
    }
}
