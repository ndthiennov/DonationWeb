namespace DonationAppDemo.DTOs
{
    public class CommentDto
    {
        public string? Content { get; set; }
        public int UserId { get; set; }    
        public int PostId { get; set; }
        public string? Role { get; set; }
    }
}