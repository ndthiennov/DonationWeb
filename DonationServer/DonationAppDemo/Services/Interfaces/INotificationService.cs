using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface INotificationService
    {
        Task<int> CheckReadLatestNotification();
        Task<List<Notification>?> Get(int pageIndex);
        Task<bool> UpdateRead(int notificationId);
        Task<bool> AddList(List<int> userIds, Notification notification);
        Task<bool> Add(Notification notification);
        Task<bool> SendMultipleNotifications(List<int>? userIds, string userRole, string notificationTitle, string notificationBody);
    }
}
