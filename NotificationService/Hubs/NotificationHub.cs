using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Dtos;
using NotificationService.Models;
using NotificationService.Repository.NotificationRepository;
using System.Collections.Concurrent;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationHub(INotificationRepository notificationRepository , IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }


        public static ConcurrentDictionary<string, string> ConnectedUsers = new();

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.TryRemove(userId, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }


        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = Context.ConnectionId;
                var unreadNotifications = await _notificationRepository.GetUnreadNotifications(Guid.Parse(userId));
                foreach (var notification in unreadNotifications)
                {
                   var ntfctn = _mapper.Map<NotificationViewDto>(notification);
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", ntfctn);
                    notification.IsRead = true;
                    await _notificationRepository.UpdateNotification(notification);
                }
            }

            await base.OnConnectedAsync();
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
