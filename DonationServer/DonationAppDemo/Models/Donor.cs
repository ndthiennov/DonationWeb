using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        //public int? District { get; set; } //optional
        //public int? City { get; set; } //optional
        public string? AvaSrc { get; set; }
        public string? AvaSrcPublicId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string? AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public ICollection<RateCampaign>? RateCampaigns { get; set; }
        public ICollection<Donation>? Donations { get; set; }
        [NotMapped]
        public ICollection<Notification>? Notifications { get; set; }
        [NotMapped]
        public ICollection<CommentPost>? CommentPosts { get; set; }
        public ICollection<CampaignParticipant>? CampaignParticipants { get; set; }
    }
}
