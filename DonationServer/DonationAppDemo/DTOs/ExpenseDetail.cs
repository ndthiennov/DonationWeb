namespace DonationAppDemo.DTOs
{
    public class ExpenseDetail
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double Amount { get; set; }
        public int OrganiserId { get; set; }
        public int CampaignId { get; set; }

    }
}
