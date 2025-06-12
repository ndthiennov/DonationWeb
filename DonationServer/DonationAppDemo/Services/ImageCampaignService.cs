using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Services.Interfaces;

namespace DonationAppDemo.Services
{
    public class ImageCampaignService : IImageCampaignService
    {
        private readonly IImageCampaignDal _imageCampaignDal;

        public ImageCampaignService(IImageCampaignDal imageCampaignDal)
        {
            _imageCampaignDal = imageCampaignDal;
        }
        public async Task<List<ImageCampaignDto>?> GetAll(int pageIndex, int campaignId, int campaignStatusId)
        {
            var images = await _imageCampaignDal.GetAll(pageIndex, campaignId, campaignStatusId);
            return images;
        }
    }
}
