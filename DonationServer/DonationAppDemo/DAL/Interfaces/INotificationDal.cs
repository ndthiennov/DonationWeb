using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface INotificationDal
    {
        Task<int> GetLatestNotification(int userId, string userRole);
        Task<List<Notification>?> Get(int userId, string userRole, int pageIndex);
        Task<bool> UpdateMarked(Notification? notification);
        Task<bool> UpdateRead(int notificationId);
        Task<bool> AddList(List<int> userIds, Notification notification);
        Task<bool> Add(Notification notificationDto);
    }
}
