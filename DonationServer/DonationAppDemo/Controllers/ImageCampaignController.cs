using DonationAppDemo.Services;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageCampaignController : ControllerBase
    {
        private readonly IImageCampaignService _imageCampaignService;

        public ImageCampaignController(IImageCampaignService imageCampaignService)
        {
            _imageCampaignService = imageCampaignService;
        }

        [HttpGet]
        [Route("GetAll/{pageIndex}/{campaignId}/{campaignStatusId}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageIndex, [FromRoute] int campaignId, [FromRoute] int campaignStatusId)
        {
            try
            {
                var result = await _imageCampaignService.GetAll(pageIndex, campaignId, campaignStatusId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
