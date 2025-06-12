using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class CampaignParticipant
    {
        public int CampaignId { get; set; }
        public int DonorId { get; set; }
        public virtual Campaign? Campaign { get; set; }
        public virtual Donor? Donor { get; set; }
    }
}
