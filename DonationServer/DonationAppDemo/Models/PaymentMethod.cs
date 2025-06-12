﻿namespace DonationAppDemo.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string? Method { get; set; }
        public ICollection<Donation>? Donations { get; set; }
    }
}
