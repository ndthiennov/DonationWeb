namespace DonationAppDemo.DTOs
{
    public class CampaignShortBDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Target { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Address { get; set; }
        //public string? City { get; set; }
        public string? CoverSrc { get; set; }
        public string? Status { get; set; }
        public int? OrganiserId { get; set; }
        public string? OrganiserName { get; set; }
        public string? OrganiserAva { get; set; }
        public int? RecipientId { get; set; }
        public string? RecipientName { get; set; }
        public string? Received { get; set; }
    }
}
