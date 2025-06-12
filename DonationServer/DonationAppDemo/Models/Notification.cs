using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? NotificationTitle { get; set; }
        public string? NotificationText { get; set;}
        public DateTime? NotificationDate { get; set; }
        public bool? IsRead { get; set;} // unread == 0 / read == 1
        public bool? Marked { get; set; } // The last time of opening notification box
        public int? FromUserId { get; set; }
        public string? FromUserRole { get; set; }
        public int? ToUserId { get; set; }
        public string? ToUserRole { get; set; }
        [NotMapped]
        public virtual Admin? FromAdmin { get; set; }
        [NotMapped]
        public virtual Organiser? FromOrganiser { get; set; }
        [NotMapped]
        public virtual Organiser? ToOrganiser { get; set; }
        [NotMapped]
        public virtual Donor? ToDonor { get; set; }
        [NotMapped]
        public virtual Recipient? ToRecipient { get; set; }
    }
}
