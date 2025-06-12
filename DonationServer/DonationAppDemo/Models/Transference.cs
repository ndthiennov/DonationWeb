using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Transference
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Amount { get; set; }
        public int? AdminId { get; set; }
        public int? CampaignId { get; set; }
        public virtual Admin? Admin { get; set; }
        public virtual Campaign? Campaign { get; set; }
    }
}
