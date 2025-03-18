using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace ChatService.Model
{
    public class ChatMessage
    {
        [Key] 
        public Guid ChatMessageId { get; set; }

        [Required]
        public Guid SenderId { get; set; }  // Changed to Guid

        [Required]
        public Guid ReceiverId { get; set; }  // Changed to Guid

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
