using DonationAppDemo.DTOs;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IImageCampaignService
    {
        Task<List<ImageCampaignDto>?> GetAll(int pageIndex, int campaignId, int campaignStatusId);
    }
}
