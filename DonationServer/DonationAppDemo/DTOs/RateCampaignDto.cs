using DonationAppDemo.Models;

namespace DonationAppDemo.DTOs
{
    public class RateCampaignDto
    {
        public int CampaignId { get; set; }
        public int DonorId { get; set; }
        public string? DonorName { get; set; }
        public string? DonorAva { get; set; }
        public int? Rate { get; set; }
        public string? Content { get; set; }
        public string? RatedDate { get; set; }
    }
}
