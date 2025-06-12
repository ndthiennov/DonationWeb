using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;

namespace DonationAppDemo.Services
{
    public class CampaignStatisticsService : ICampaignStatisticsService
    {
        private readonly ICampaignStatisticsDal _campaignStatisticsDal;

        public CampaignStatisticsService(ICampaignStatisticsDal campaignStatisticsDal)
        {
            _campaignStatisticsDal = campaignStatisticsDal;
        }
        public async Task<CampaignStatistics?> GetById(int campaignId)
        {
            var result = await _campaignStatisticsDal.GetById(campaignId);
            return result;
        }
    }
}
