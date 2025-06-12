using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class ImagePost
    {
        public int Id { get; set; }
        public string? ImageSrc { get; set; }
        public string? ImageSrcPublicId { get; set; }
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
        public string ImagePath { get; internal set; }
    }
}
