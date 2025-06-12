using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public DateTime? DonationDate { get; set; }
        public decimal? Amount { get; set; }
        public int? DonorId { get; set; }
        public int? CampaignId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentDescription { get; set; }
        public string? PaymentOrderId { get; set; }
        public string? PaymentTransactionId { get; set; }
        public string? PaymentToken { get; set; }
        public string? PaymentResponse { get; set; }
        public virtual Donor? Donor { get; set; }
        public virtual Campaign? Campaign { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
