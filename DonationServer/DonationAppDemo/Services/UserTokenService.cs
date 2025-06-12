using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenDal _userTokenDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTokenService(IUserTokenDal userTokenDal,
            IHttpContextAccessor httpContextAccessor)
        {
            _userTokenDal = userTokenDal;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<string?>?> GetTokenList(List<int> userIds, string userRole)
        {
            var result = await _userTokenDal.GetTokenList(userIds, userRole);
            return result;
        }
        public async Task<bool> Add(string fcmToken)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();
            var currentUserRole = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value.ToString();

            // Check fcm token existed
            List<int> userIds = new List<int>();
            userIds.Add(Int32.Parse(currentUserId));
            var tokens = await _userTokenDal.GetTokenList(userIds, currentUserRole);
            if(tokens != null)
            {
                foreach (var token in tokens)
                {
                    if (token == fcmToken)
                    {
                        throw new Exception("Fcm token existed");
                    }
                }
            }
    
            var userToken = new UserToken
            {
                UserId = Int32.Parse(currentUserId),
                UserRole = currentUserRole,
                FcmToken = fcmToken,
                UpdatedDate = DateTime.Now,
            };

            var result = await _userTokenDal.Add(userToken);

            return result;
        }
    }
}
