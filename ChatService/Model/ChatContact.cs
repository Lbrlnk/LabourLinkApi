using System.ComponentModel.DataAnnotations;

namespace ChatService.Model
{
    public class ChatContact
    {
       
            [Key]
            public Guid Id { get; set; }
            [Required]
            public Guid User1Id { get; set; }
            [Required]
            public Guid User2Id { get; set; }

            [Required]
            public string User1Name { get; set; }

            [Required]

            public string User2Name { get; set; }


            public string? User1Image { get; set; }

            public string? User2Image { get; set; }

            public string LastMessage { get; set; }
            public DateTime LastUpdated { get; set; } = DateTime.Now;

            public bool IsBlocked { get; set; } = false;  // True if chat is blocked
            public Guid? BlockedByUserId { get; set; }  // Stores the user who blocked


            
        
    }
}
