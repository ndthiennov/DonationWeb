using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IUserTokenDal
    {
        Task<List<string?>?> GetTokenList(List<int> userIds, string userRole);
        Task<bool> Add(UserToken userTokenDto);
    }
}
