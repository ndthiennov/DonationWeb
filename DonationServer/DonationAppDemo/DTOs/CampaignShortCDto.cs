namespace DonationAppDemo.DTOs
{
    public class CampaignShortCDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Target { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAva { get; set; }
        public string? Received { get; set; }
        public string? CreatedDate { get; set; }
        public string? Disabled { get; set; }
    }
}
