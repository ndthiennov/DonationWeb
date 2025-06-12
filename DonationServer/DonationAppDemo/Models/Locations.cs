namespace DonationAppDemo.Models
{
    public class Locations
    {
        public int Id { get; set; }
        public string? Add_Campaign { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Created_date { get; set; }
        public int Id_Campaign { get; set; }
    }
}
