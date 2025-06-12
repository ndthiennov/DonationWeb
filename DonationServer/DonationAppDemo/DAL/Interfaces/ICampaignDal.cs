using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface ICampaignDal
    {
        Task<List<CampaignShortADto>?> GetListByAdmin(int pageIndex);
        Task<List<CampaignShortADto>?> GetSearchedListByAdmin(int pageIndex, CampaignSearchADto search);
        Task<List<CampaignShortBDto>?> GetSearchedListByUser(int pageIndex, CampaignSearchADto search);
        Task<List<CampaignShortCDto>?> GetSearchedListByOrganiser(int pageIndex, CampaignSearchADto search, int organiserId);
        Task<List<CampaignShortBDto>?> GetSearchedListByRecipient(int pageIndex, int recipientId, CampaignSearchADto search);
        Task<CampaignDetailBDto?> GetById(int campaignId);
        Task<Campaign> Add(CampaignCUDto campaignCUDto, string? coverPublicId, string? coverSrc, int? organiserId);
        Task<Campaign> Update(int campaignId, CampaignCUDto campaignCUDto, string? coverPublicId, string? coverSrc, int? organiserId);
        Task<bool> UpdateDisabledCampaign(int campaignId, bool disabled);
        Task<bool> UpdateRecivedByRecipient(int campaignId, int recipientId, bool received);
        Task<bool> UpdateRatedByRecipient(int campaignId, int recipientId, RateCampaign rateCampaign);
        /*Task<Campaign?> Get(int campaignId);
        Task<Campaign> Add(Campaign campaign);
        Task<Campaign> Update(CampaignDto campaignDto);
        Task<bool> Remove(int campaignId);
        Task<bool> ChangeStatus(int campaignId, int statusId);*/
    }
}
