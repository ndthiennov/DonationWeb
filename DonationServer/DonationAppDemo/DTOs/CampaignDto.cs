using DonationAppDemo.Models;

namespace DonationAppDemo.DTOs
{
    public class CampaignDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Target { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public IFormFile? CoverImage { get; set; }
        public decimal? TargetAmount { get; set; }
        public int? OrganiserId { get; set; }
        public virtual StatusCampaign? StatusCampaign { get; set; }
        public virtual Organiser? Organiser { get; set; }
    }
}
