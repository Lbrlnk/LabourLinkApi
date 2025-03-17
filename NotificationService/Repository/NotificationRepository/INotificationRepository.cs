using NotificationService.Models;

namespace NotificationService.Repository.NotificationRepository
{
    public interface INotificationRepository
    {
        Task<bool> AddNotification(Notification notification);

        Task<bool> UpdateNotification(Notification notification);

        Task<List<Notification>> GetUnreadNotifications(Guid userId);
    }
}
