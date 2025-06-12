namespace DonationAppDemo.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<List<string?>?> GetTokenList(List<int> userIds, string userRole);
        Task<bool> Add(string fcmToken);
    }
}
