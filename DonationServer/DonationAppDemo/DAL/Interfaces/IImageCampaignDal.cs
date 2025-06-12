using DonationAppDemo.Models;
using DonationAppDemo.DTOs;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IImageCampaignDal
    {
        Task<List<ImageCampaignDto>?> GetAll(int pageIndex, int campaignId, int campaignStatusId);
        /*Task<List<ImageCampaign>> AddImages(List<ImageCampaign> imageCamapaigns);
        Task<bool> Remove(int imageId);
        Task<bool> RemoveByCampaignId(int campaignId);
        Task<List<ImageCampaign>> GetById(int campaignId, int pageIndex);
        Task<List<ImageCampaign>> GetAllById(int campaignId);
        Task<bool> RemoveListImages(List<ImageCampaignDto> imageCampaignDtos);*/
    }
}
