using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDal _notificationDal;
        private readonly IUserTokenService _userTokenService;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(INotificationDal notificationDal,
            IUserTokenService userTokenService,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _notificationDal = notificationDal;
            _userTokenService = userTokenService;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> CheckReadLatestNotification()
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();
            var currentUserRole = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value.ToString();

            var notificationCount = await _notificationDal.GetLatestNotification(Int32.Parse(currentUserId), currentUserRole);

            /*if(notification == null)
            {
                return true;
            }

            if (notification.Marked == true)
            {
                return true;
            }*/

            return notificationCount;
        }
        public async Task<List<Notification>?> Get(int pageIndex)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();
            var currentUserRole = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value.ToString();

            // Query
            var notifications = await _notificationDal.Get(Int32.Parse(currentUserId), currentUserRole, pageIndex);

            if (pageIndex == 1)
            {
                if(notifications != null)
                {
                    // Update marked of last notification in notifications for marking the last notification that user opened in notification box
                    await _notificationDal.UpdateMarked(notifications.FirstOrDefault());
                }
            }
            return notifications;
        }
        public async Task<bool> UpdateRead(int notificationId)
        {
            var result = await _notificationDal.UpdateRead(notificationId);
            if (!result)
            {
                throw new Exception($"There is no notification id {notificationId}");
            }
            return true;
        }
        public async Task<bool> AddList(List<int>? userIds, Notification notification)
        {
            if(userIds == null)
            {
                return true;
            }

            var result = await _notificationDal.AddList(userIds, notification);
            if (!result)
            {
                throw new Exception($"Fail to add notification list");
            }
            return true;
        }
        public async Task<bool> Add(Notification notification)
        {
            var result = await _notificationDal.Add(notification);
            if (!result)
            {
                throw new Exception($"Fail to add notification");
            }
            return true;
        }
        public async Task<bool> SendMultipleNotifications(List<int>? userIds, string userRole, string notificationTitle, string notificationBody)
        {
            if (userIds == null)
            {
                return true;
            }

            var tokens = await _userTokenService.GetTokenList(userIds, userRole);

            if(tokens != null && tokens.Count() > 0)
            {
                await _utilitiesService.SendMultipleNotifications(tokens, notificationTitle, notificationBody);
            }

            return true;
        }
    }
}
