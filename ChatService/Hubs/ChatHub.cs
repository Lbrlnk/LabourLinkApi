using ChatService.Model;
using ChatService.Services.ChatService;
using ChatService.Services.ConversationServices;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IChatMessageService _chatService;
        private readonly IConversationService _conversationService;
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

        public ChatHub(IChatMessageService chatService, IConversationService chatConversationService)
        {
            _chatService = chatService;
            _conversationService = chatConversationService;
        }

        public override Task OnConnectedAsync()
        {
            var refId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            string userId = refId.ToString();
            Console.WriteLine("===============>", userId);

            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = Context.ConnectionId;
                Console.WriteLine($"User {userId} connected with connection ID {Context.ConnectionId}");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.TryRemove(userId, out _);
                Console.WriteLine($"User {userId} disconnected");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("User not authenticated.");
            }

            var chatMessage = new ChatMessage
            {
                SenderId = Guid.Parse(senderId),
                ReceiverId = Guid.Parse(receiverId),
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await _chatService.SaveChatMessages(chatMessage);
            await _conversationService.UpdateLastMessage(Guid.Parse(senderId), Guid.Parse(receiverId), message);


            if (ConnectedUsers.TryGetValue(receiverId, out string connectionId))
            {
                Console.WriteLine("Sendiiinggg");
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
            }
            Console.WriteLine($"Sending message back to sender: {senderId}");

            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
