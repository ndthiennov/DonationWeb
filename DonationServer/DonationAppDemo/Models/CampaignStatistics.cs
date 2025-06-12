using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class CampaignStatistics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CampaignId { get; set; }
        public decimal? TotalDonationAmount { get; set; }
        public decimal? TotalTransferredAmount { get; set; }
        public decimal? TotalExpendedAmount { get; set; }
    }
}
