using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid User1Id { get; set; }
        [Required]
        public Guid User2Id { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public bool IsBlocked { get; set; } = false;  // True if chat is blocked
        public Guid? BlockedByUserId { get; set; }  // Stores the user who blocked


        public virtual Employer User1 { get; set; }
        public virtual Labour User2 { get; set; }
    }
}
