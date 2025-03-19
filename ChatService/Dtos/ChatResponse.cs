namespace ChatService.Dtos
{
    public class ChatResponse
    {

        public Guid MessageId  {get; set; }


        public Guid SenderId { get; set; }


        public Guid ReceiverId { get; set; }


        public string Message { get; set; }


        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsYouSender { get; set; } = false;
    }
}
