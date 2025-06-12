using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces

{
    public interface IRateCampaignDal
    {
        Task<List<RateCampaignDto>> GetListByCampaignId(int campaignId, int pageIndex);
        Task<bool> Add(RateCampaign rateCampaign);
        Task<bool> RemoveByDonorId(int campaignId, int donorId);
        /*Task<bool> CheckExistedRateCampaign(RateCampaignDto rateCampaignDto);
        Task<RateCampaign> Add(RateCampaign rateCampaign);
        Task<RateCampaign> Update(RateCampaignDto rateCampaignDto);
        Task<bool> RemoveByCampaignId(int campaignId);
        Task<List<RateCampaign>> GetById(int campaignId, int pageSize, int pageIndex);
        Task<List<RateCampaign>> GetAllById(int campaignId);*/
    }
}
