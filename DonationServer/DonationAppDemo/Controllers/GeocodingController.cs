using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DonationAppDemo.Controllers
{
    public class GeocodingController : ControllerBase
    {
        private readonly IGeocodingService _geocodingService;

        public GeocodingController(IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;

        }
        /// <summary>
        /// API để lấy kết quả theo ngày từ bảng Locations.
        /// </summary>
        [HttpGet("Get-locations")]
        public async Task<IActionResult> GetLocationsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                // Lọc theo khoảng thời gian
                var locations = await _geocodingService.GetLocationsByDateRangeAsync(startDate, endDate);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving locations", error = ex.Message });
            }
        }
    }
}
