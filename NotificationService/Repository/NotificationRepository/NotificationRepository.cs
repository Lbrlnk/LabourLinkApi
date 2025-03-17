using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Models;

namespace NotificationService.Repository.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       public async Task<bool> AddNotification(Notification notification)
        {
           await _context.Notifications.AddAsync(notification);
           return await _context.SaveChangesAsync() > 0;
            //return true;
        }

        public async Task<bool> UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Notification>> GetUnreadNotifications(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverUserId == userId && !n.IsRead)
                .ToListAsync();
        }
    }
}
