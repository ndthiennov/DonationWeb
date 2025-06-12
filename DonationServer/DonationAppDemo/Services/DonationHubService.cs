using DonationAppDemo.DTOs;
using DonationAppDemo.HubConfig;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Twilio.Http;

namespace DonationAppDemo.Services
{
    public class DonationHubService : IDonationHubService
    {
        private readonly IHubContext<DonationHub> _hubConext;

        public DonationHubService(IHubContext<DonationHub> hubConext)
        {
            _hubConext = hubConext;
        }
        public async Task SendDonation(DonationDto donationDto)
        {
            await _hubConext.Clients.All.SendAsync($"Campaign:{donationDto.CampaignId}", donationDto);
        }
    }
}
