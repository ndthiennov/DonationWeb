using Microsoft.AspNetCore.Http;
using System;

namespace DonationAppDemo.DTOs
{
    public class PostDto
    {
        public string? ContentPost { get; set; }
        public IFormFile? Image { get; set; }
        public int AdminId { get; set; }
    }
}