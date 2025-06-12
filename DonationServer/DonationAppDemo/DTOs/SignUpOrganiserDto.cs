namespace DonationAppDemo.DTOs
{
    public class SignUpOrganiserDto
    {
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Dob { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public IFormFile? CertificationFile { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? Disabled { get; set; }
    }
}
