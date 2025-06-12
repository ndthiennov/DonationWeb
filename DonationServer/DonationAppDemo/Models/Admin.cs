using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string? AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public ICollection<Transference>? Transferences { get; set; }
        [NotMapped]
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Post>? Posts { get; set; }
        [NotMapped]
        public ICollection<CommentPost>? CommentPosts { get; set; }

    }
}
