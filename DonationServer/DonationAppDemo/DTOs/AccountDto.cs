namespace DonationAppDemo.DTOs
{
    public class AccountDto
    {
        public string? PhoneNum { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Role { get; set; }
        public bool? Disabled { get; set; }
    }
}
