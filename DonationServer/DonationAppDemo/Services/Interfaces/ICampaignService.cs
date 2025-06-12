using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface ICampaignService
    {
        Task<List<CampaignShortADto>?> GetListByAdmin(int pageIndex);
        Task<List<CampaignShortADto>?> GetSearchedListByAdmin(int pageIndex, CampaignSearchADto search);
        Task<List<CampaignShortBDto>?> GetSearchedListByUser(int pageIndex, CampaignSearchADto search);
        Task<List<CampaignShortCDto>?> GetSearchedListByOrganiser(int pageIndex, CampaignSearchADto search);
        Task<List<CampaignShortBDto>?> GetSearchedListByRecipient(int pageIndex, CampaignSearchADto search);
        Task<CampaignDetailBDto?> GetById(int campaignId);
        Task<CampaignShortCDto> Add(CampaignCUDto campaignCUDto);
        Task<CampaignShortCDto> Update(int campaignId, CampaignCUDto campaignCUDto);
        Task<bool> UpdateDisabledCampaign(int campaignId, bool disabled);
        Task<bool> UpdateRecivedByRecipient(int campaignId, bool received);
        Task<bool> UpdateRatedByRecipient(int campaignId, RateCampaign rateCampaign);
        //Task<Campaign> CreateCampaign(CampaignDto campaignDto);
        //Task<Campaign?> Get(int campaignId);
        //Task<bool> DeleteCampaign(CampaignDto campaignDto);
        //Task<RateCampaign> RateCampaign(RateCampaignDto rateCampaignDto);
        //Task<List<ImageCampaign>> AddListImageCampaign(List<ImageCampaignDto> listImageCampaignDto);
        //Task<Campaign> UpdateCampaign(CampaignDto campaignDto);
        //Task<bool> ChangeStatusCampaign(int campaignId, int statusId);
        //Task<RateCampaign> UpdateRateCampaign(RateCampaignDto rateCampaignDto);
        //Task<bool> RemoveListImageCampaign(List<ImageCampaignDto> listImageCampaignDto);
        MemoryStream GenerateExcelReportDonations(int campaignId,DateTime startDate, DateTime endDate);
        MemoryStream GenerateExcelReportExpense(int campaignId,DateTime startDate, DateTime endDate);

    }
}
