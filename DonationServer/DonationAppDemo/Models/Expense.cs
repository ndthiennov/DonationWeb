using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public decimal? Amount { get; set; }
        public int OrganiserId { get; set; }
        public int CampaignId { get; set; }
        public virtual Organiser? Organiser { get; set; }
        public virtual Campaign? Campaign { get; set; }
    }
}
