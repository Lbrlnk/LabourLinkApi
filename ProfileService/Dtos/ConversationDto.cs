using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        
        public Guid User1Id { get; set; }
       
        public Guid User2Id { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public bool IsBlocked { get; set; } = false;  // True if chat is blocked
        public Guid? BlockedByUserId { get; set; }  // Stores the user who blocked
    }
}
