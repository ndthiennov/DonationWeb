namespace DonationAppDemo.DAL.Interfaces
{
    public interface ICampaignParticipantDal
    {
        Task<List<int>?> GetAllDonorIdByCampaignId(int campaignId);
        Task<bool> CheckParticipated(int donorId, int campaignId);
        Task<bool> JoinCampaign(int campaignId, int donorId);
        Task<bool> CancelCampaignPartipation(int campaignId, int donorId);
    }
}
