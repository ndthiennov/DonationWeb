namespace DonationAppDemo.Services.Interfaces
{
    public interface ICampaignParticipantService
    {
        Task<List<int>?> GetAllDonorIdByCampaignId(int campaignId);
        Task<bool> CheckParticipated(int campaignId);
        Task<bool> JoinCampaign(int campaignId);
        Task<bool> CancelCampaignPartipation(int campaignId);
    }
}
