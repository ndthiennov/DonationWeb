using DonationAppDemo.Services;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignStatisticsController : ControllerBase
    {
        private readonly ICampaignStatisticsService _campaignStatisticsService;

        public CampaignStatisticsController(ICampaignStatisticsService campaignStatisticsService)
        {
            _campaignStatisticsService = campaignStatisticsService;
        }

        [HttpGet]
        [Route("GetById/{campaignId}")]
        public async Task<IActionResult> GetById([FromRoute]int campaignId)
        {
            try
            {
                var result = await _campaignStatisticsService.GetById(campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
