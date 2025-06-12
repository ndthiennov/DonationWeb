namespace DonationAppDemo.DTOs
{
    public class TransferenceDto
    {
        public string? Description { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public int AdminId { get; set; }
        public int CampaignId { get; set; }
    }
}