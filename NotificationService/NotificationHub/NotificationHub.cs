using Microsoft.AspNetCore.SignalR;
using NotificationService.Models;

namespace NotificationService.NotificationHub
{
    public class NotificationHub : Hub
    {
        private static Dictionary<string, string> ConnectedUsers = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.Remove(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(Guid userId, Notification notification)
        {
            if (ConnectedUsers.TryGetValue(userId.ToString(), out string connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
            }
        }
    }

}
