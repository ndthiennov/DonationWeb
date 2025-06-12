namespace DonationAppDemo.DTOs
{
    public class CampaignSearchADto
    {
        public string Campaign { get; set; } = string.Empty; // name or id
        public string User { get; set; } = string.Empty; // name or id
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

    }
}
