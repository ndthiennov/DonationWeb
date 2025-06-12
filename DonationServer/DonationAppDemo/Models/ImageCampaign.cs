using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class ImageCampaign
    {
        public int Id { get; set; }
        public string? ImageSrc { get; set; }
        public string? ImageSrcPublicId { get; set; }
        public int? CampaignId { get; set; }
        public int? StatusCampaignId { get; set; }
        public virtual Campaign? Campaign { get; set; }
        public virtual StatusCampaign? StatusCampaign { get; set; }
    }
}
