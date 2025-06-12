namespace DonationAppDemo.DTOs
{
    public class DonationDetail
    {
        public int Id { get; set; }
        public DateTime DonationDate { get; set; }
        public double Amount { get; set; }
        public int DonorId { get; set; }
        public int CampaignId { get; set; }
        public string? PaymentDescription { get; set; }

    }
}
