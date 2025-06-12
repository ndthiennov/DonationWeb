using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface ICampaignStatisticsService
    {
        Task<CampaignStatistics?> GetById(int campaignId);
    }
}
