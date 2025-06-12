using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationAppDemo.Models
{
    public class Account
    {
        [Key]
        public string PhoneNum { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } //admin, donor, organiser, recipient
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set;}
        public bool? Disabled { get; set; }
        public ICollection<Admin>? Admins { get; set; }
        public ICollection<Organiser>? Organisers { get; set; }
        public ICollection<Donor>? Donors { get; set; }
    }
}
