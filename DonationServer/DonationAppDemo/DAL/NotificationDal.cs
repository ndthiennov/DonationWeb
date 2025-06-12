using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class NotificationDal : INotificationDal
    {
        private readonly DonationDbContext _context;

        public NotificationDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetLatestNotification(int userId, string userRole)
        {
            /*var latest = await _context.Notification
                .Where(x => x.ToUserId == userId && x.ToUserRole == userRole)
                .OrderByDescending(x => x.NotificationDate)
                .FirstOrDefaultAsync();
            return latest;*/

            // Get latest marked opening
            var latest = await _context.Notification
                .Where(m => m.Marked == true && m.ToUserId == userId && m.ToUserRole == userRole)
                .OrderByDescending(m => m.NotificationDate)
                .FirstOrDefaultAsync();

            if(latest == null)
            {
                latest = new Notification();
                latest.NotificationDate = DateTime.MinValue;
            }

            var count = await _context.Notification
                .Where(x => x.ToUserId == userId && x.ToUserRole == userRole && x.IsRead == false && x.NotificationDate > latest.NotificationDate)
                .CountAsync();

            return count;
        }
        public async Task<List<Notification>?> Get(int userId, string userRole, int pageIndex)
        {
            if (pageIndex == 1)
            {
                var notifications = await _context.Notification
                .Where(x => x.ToUserId == userId && x.ToUserRole == userRole)
                .OrderByDescending(x => x.NotificationDate)
                .Skip((pageIndex - 1) * 3)
                .Take(3)
                .ToListAsync();
                return notifications;
            }
            else // pageIndex > 1
            {
                var notifications = await _context.Notification
                .Where(x => x.ToUserId == userId && x.ToUserRole == userRole &&
                x.NotificationDate <=
                _context.Notification
                    .Where(z => z.ToUserId == userId && z.ToUserRole == userRole && z.Marked == true)
                    .Max(z => z.NotificationDate))
                .OrderByDescending(x => x.NotificationDate)
                .Skip((pageIndex - 1) * 3)
                .Take(3)
                .ToListAsync();
                return notifications;
            }
        }
        public async Task<bool> UpdateMarked(Notification? notification)
        {
            if(notification == null)
            {
                return false;
            }

            notification.Marked = true;

            _context.Notification.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRead(int notificationId)
        {
            var notification = await _context.Notification.Where(x => x.Id == notificationId).FirstOrDefaultAsync();

            if(notification == null)
            {
                return false;
            }

            notification.IsRead = true;

            _context.Notification.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddList(List<int> userIds, Notification notification)
        {
            var notifications = userIds.Select(userId => new Notification
            {
                NotificationTitle = notification.NotificationTitle,
                NotificationText = notification.NotificationText,
                NotificationDate = notification.NotificationDate,
                IsRead = false,
                Marked = false,
                FromUserId = notification.FromUserId,
                FromUserRole = notification.FromUserRole,
                ToUserId = userId,
                ToUserRole = notification.ToUserRole,
            });

            _context.Notification.AddRange(notifications);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Add(Notification notificationDto)
        {
            var notification = new Notification
            {
                NotificationTitle = notificationDto.NotificationTitle,
                NotificationText = notificationDto.NotificationText,
                NotificationDate = notificationDto.NotificationDate,
                IsRead = false,
                Marked = false,
                FromUserId = notificationDto.FromUserId,
                FromUserRole = notificationDto.FromUserRole,
                ToUserId = notificationDto.ToUserId,
                ToUserRole = notificationDto.ToUserRole
            };

            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
