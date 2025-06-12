using DonationAppDemo.DTOs;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IDonationHubService
    {
        Task SendDonation(DonationDto donationDto);
    }
}
