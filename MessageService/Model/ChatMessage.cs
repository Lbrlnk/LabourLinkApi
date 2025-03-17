using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace ChatService.Model
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid MessageId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid SenderId { get; set; }  // Changed to Guid

        [BsonRepresentation(BsonType.String)]
        public Guid ReceiverId { get; set; }  // Changed to Guid

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
