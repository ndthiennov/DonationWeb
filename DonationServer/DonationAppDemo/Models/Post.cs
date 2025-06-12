using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? ContentPost { get; set; }
        public DateTime PostDate { get; set; }
        public bool? Disabled { get; set; }
        public int? AdminId { get; set; }
        public virtual Admin? Admin { get; set; }
        public ICollection<ImagePost>? ImagePosts { get; set;}
        public ICollection<CommentPost>? CommentPosts { get; set; }
    }
}
