using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class RateCampaign
    {
        public int CampaignId { get; set; } //primary key
        public int DonorId { get; set; } //primary key
        public int? Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime? RatedDate { get; set; }
        public virtual Campaign? Campaign { get; set; }
        public virtual Donor? Donor { get; set; }
    }
}
