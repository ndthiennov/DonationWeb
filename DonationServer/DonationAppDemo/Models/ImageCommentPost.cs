using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class ImageCommentPost
    {
        public int Id { get; set; }
        public string? ImageSrc { get; set; }
        public string? ImageSrcPublicId { get; set; }
        public int? CommentPostId { get; set; }
        public virtual CommentPost? CommentPost { get; set; }
    }
}
