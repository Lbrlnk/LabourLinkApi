//using ChatService.Model;
//using ChatService.Services.ChatService;
//using ChatService.Services.ConversationServices;
//using Microsoft.AspNetCore.SignalR;
//using System.Collections.Concurrent;
//using System.Security.Claims;

//namespace ChatService.Hubs
//{
//    public class ChatHub : Hub
//    {

//        private readonly IChatMessageService _chatService;
//        private readonly IConversationService _conversationService;
//        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

//        public ChatHub(IChatMessageService chatService, IConversationService chatConversationService)
//        {
//            _chatService = chatService;
//            _conversationService = chatConversationService;
//        }

//        public override Task OnConnectedAsync()
//        {
//            var refId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
//            string userId = refId.ToString();
//            Console.WriteLine("===============>", userId);

//            if (!string.IsNullOrEmpty(userId))
//            {
//                ConnectedUsers[userId] = Context.ConnectionId;
//                Console.WriteLine($"User {userId} connected with connection ID {Context.ConnectionId}");
//            }

//            return base.OnConnectedAsync();
//        }

//        public override Task OnDisconnectedAsync(Exception? exception)
//        {
//            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (!string.IsNullOrEmpty(userId))
//            {
//                ConnectedUsers.TryRemove(userId, out _);
//                Console.WriteLine($"User {userId} disconnected");
//            }

//            return base.OnDisconnectedAsync(exception);
//        }

//        public async Task SendMessage(string receiverId, string message)
//        {
//            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(senderId))
//            {
//                throw new HubException("User not authenticated.");
//            }

//            var chatMessage = new ChatMessage
//            {
//                SenderId = Guid.Parse(senderId),
//                ReceiverId = Guid.Parse(receiverId),
//                Message = message,
//                Timestamp = DateTime.UtcNow
//            };

//            await _chatService.SaveChatMessages(chatMessage);
//            await _conversationService.UpdateLastMessage(Guid.Parse(senderId), Guid.Parse(receiverId), message);


//            if (ConnectedUsers.TryGetValue(receiverId, out string connectionId))
//            {
//                Console.WriteLine("Sendiiinggg");
//                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
//            }
//            Console.WriteLine($"Sending message back to sender: {senderId}");

//            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
//        }
//    }
//}


using ChatService.Model;
using ChatService.Services.ChatService;
using ChatService.Services.ConversationServices;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
//using Microsoft.Azure.SignalR.Management;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageService _chatService;
        private readonly IConversationService _conversationService;
        private static readonly ConcurrentDictionary<string, List<string>> UserConnectionMap = new();

        public ChatHub(IChatMessageService chatService, IConversationService chatConversationService)
        {
            _chatService = chatService;
            _conversationService = chatConversationService;
        }

        public override Task OnConnectedAsync()
        {
            var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;
                var connectionId = Context.ConnectionId;

                // Add user to connection mapping
                UserConnectionMap.AddOrUpdate(
                    userId,
                    new List<string> { connectionId },
                    (key, existingConnections) =>
                    {
                        if (!existingConnections.Contains(connectionId))
                        {
                            existingConnections.Add(connectionId);
                        }
                        return existingConnections;
                    });

                // Add to group based on user ID for easier messaging
                Groups.AddToGroupAsync(connectionId, $"user-{userId}");

                Console.WriteLine($"User {userId} connected with connection ID {connectionId}");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;
                var connectionId = Context.ConnectionId;

                // Remove connection ID from the user's list of connections
                if (UserConnectionMap.TryGetValue(userId, out var connections))
                {
                    connections.Remove(connectionId);

                    // If no more connections, remove the user from the dictionary
                    if (connections.Count == 0)
                    {
                        UserConnectionMap.TryRemove(userId, out _);
                    }
                }

                // Remove from user group
                Groups.RemoveFromGroupAsync(connectionId, $"user-{userId}");

                Console.WriteLine($"User {userId} disconnected from connection {connectionId}");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (senderIdClaim == null)
            {
                throw new HubException("User not authenticated.");
            }

            var senderId = senderIdClaim.Value;

            var chatMessage = new ChatMessage
            {
                SenderId = Guid.Parse(senderId),
                ReceiverId = Guid.Parse(receiverId),
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            // Save message to database
            await _chatService.SaveChatMessages(chatMessage);

            // Update conversation with last message
            await _conversationService.UpdateLastMessage(
                Guid.Parse(senderId),
                Guid.Parse(receiverId),
                message
            );

            // Send to receiver's group instead of individual connections
            await Clients.Group($"user-{receiverId}").SendAsync("ReceiveMessage", senderId, message);

            // Send confirmation back to the sender
            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }
    }
}