using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class UserTokenDal : IUserTokenDal
    {
        private readonly DonationDbContext _context;

        public UserTokenDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<List<string?>?> GetTokenList(List<int> userIds, string userRole)
        {
            var tokens = await _context.UserToken.Where(x => x.UserRole == userRole && userIds.Contains(x.UserId)).Select(x => x.FcmToken).ToListAsync();
            return tokens;
        }
        public async Task<bool> Add(UserToken userTokenDto)
        {
            var userToken = new UserToken
            {
                UserId = userTokenDto.UserId,
                UserRole = userTokenDto.UserRole,
                FcmToken = userTokenDto.FcmToken,
                UpdatedDate = DateTime.Now
            };

            _context.UserToken.Add(userToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
