using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface ITransactionDal
    {
        Task<bool> SignUpOrganiser(AccountDto accountDto, OrganiserDto organiserDto, string? certificationPublicId);
        Task<bool> BecomeOrganiser(string phoneNum, string role, bool disabled, OrganiserDto organiserDto, string? certificationPublicId);
        Task<bool> DeleteUncensoredOrganiser(string phoneNum, int organiserId);
        Task<bool> SignUpDonor(AccountDto accountDto, DonorDto donorDto);
        Task<bool> SignUpRecipient(AccountDto accountDto, RecipientDto recipientDto);
        Task<bool> BecomeDonor(string phoneNum, string role, bool disabled, DonorDto donorDto);
        Task<bool> AccountAdmin(AccountDto accountDto, AdminDto adminDto);
        Task<CampaignStatistics?> AddDonation(PaymentResponseDto paymentResponseDto);
        Task<CampaignStatistics?> AddExpense(Expense expense);
        Task<CampaignStatistics?> DeleteExpense(Expense expense);
        //Task<bool> CampaignRateImage(CampaignDto campaignDto);
    }
}
