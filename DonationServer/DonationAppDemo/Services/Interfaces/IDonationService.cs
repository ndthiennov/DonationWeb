using DonationAppDemo.DTOs;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IDonationService
    {
        //Task<List<DonationDto>?> GetListByCampaignId(int campaignId, SearchDto searchDto);
        //Task<List<DonationDto>?> GetListByDonorId(SearchDto searchDto);
        Task<List<DonationDto>?> GetSearchedListByCampaignId(int campaignId, SearchDto searchDto);
        Task<string> CreatePaymentUrl(HttpContext context, PaymentRequestDto request);
        Task<DonationDto> PaymentExecute(IQueryCollection collections);
    }
}
