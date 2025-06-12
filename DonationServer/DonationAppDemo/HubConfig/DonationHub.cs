using DonationAppDemo.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace DonationAppDemo.HubConfig
{
    public class DonationHub : Hub
    {
        public async Task RequestDonation(DonationDto donationDto)
        {
            await Clients.All.SendAsync($"Campaign:{donationDto.CampaignId}", donationDto);
        }
    }
}
