using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IDonationDal
    {
        //Task<List<Donation>?> GetListByCampaignId(int campaignId, int pageIndex, DateTime? fromDate, DateTime? toDate, int? donorId);
        //Task<List<DonationDto>?> GetListByDonorId(int donorId, int pageIndex, DateTime? fromDate, DateTime? toDate);
        Task<List<DonationDto>?> GetSearchedListByCampaignId(int campaignId, SearchDto searchDto);
        Task<Donation> Add(PaymentResponseDto responseDto);
    }
}
