namespace DonationAppDemo.DTOs
{
    public class DonationDto
    {
        public int? CampaignId { get; set; }
        public string? CampaignName { get; set; }
        public int? DonorId { get; set; }
        public string? DonorName { get; set; }
        public string? DonorAvaSrc { get; set; }
        public string? DonationDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CampaignDonationTotal { get; set;}

    }
}
