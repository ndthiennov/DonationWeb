using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IRateCampaignService
    {
        Task<List<RateCampaignDto>?> GetListByCampaignId(int campaignId, int pageIndex);
        Task<RateCampaignDto?> Add(RateCampaign rateCampaign);
        Task<bool> RemoveByDonorId(int campaignId);
    }
}
