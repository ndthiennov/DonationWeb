namespace DonationAppDemo.DTOs
{
    public class CampaignCUDto
    {
        public string? Title { get; set; }
        public string? Target { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? StatusCampaignId { get; set; }
        public decimal? TargetAmount { get; set; }
        public IFormFile? CoverSrc { get; set; }
        public int? RecipientId { get; set; }
        public string? Disabled { get; set; }
    }
}
