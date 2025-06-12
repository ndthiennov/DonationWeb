namespace DonationAppDemo.DTOs
{
    public class SearchDto
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set;}
        public string? Donor { get; set; }
        public string? OrderBy { get; set; }
        public int PageIndex { get; set; }
    }
}
