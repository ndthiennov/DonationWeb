namespace DonationAppDemo.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public string? FcmToken { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
