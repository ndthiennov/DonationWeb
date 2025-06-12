namespace DonationAppDemo.DTOs
{
    public class HashSaltDto
    {
        public byte[] hashedCode { get; set; }
        public byte[] keyCode { get; set; }
    }
}
