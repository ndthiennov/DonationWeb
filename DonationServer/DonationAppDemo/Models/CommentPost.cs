using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class CommentPost
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public int? UserId { get; set; }
        public string? UserRole { get; set; }
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
        [NotMapped]
        public virtual Admin? Admin { get; set; }
        [NotMapped]
        public virtual Organiser? Organiser { get; set; }
        [NotMapped]
        public virtual Donor? Donor { get; set; }
        [NotMapped]
        public virtual Recipient? Recipient { get; set; }
        public ICollection<ImageCommentPost>? ImageCommentPosts { get; set; }
    }
}
