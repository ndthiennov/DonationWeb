using DonationAppDemo.Models;
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
    public class RateCampaignController : ControllerBase
    {
        private readonly IRateCampaignService _rateCampaignService;

        public RateCampaignController(IRateCampaignService rateCampaignService)
        {
            _rateCampaignService = rateCampaignService;
        }

        [HttpGet]
        [Route("GetListByCampaignId/{campaignId}/{pageIndex}")]
        public async Task<IActionResult> GetListByCampaignId([FromRoute]int campaignId, [FromRoute]int pageIndex)
        {
            try
            {
                var result = await _rateCampaignService.GetListByCampaignId(campaignId, pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "donor")]
        public async Task<IActionResult> Add([FromBody] RateCampaign rateCampaign)
        {
            try
            {
                var result = await _rateCampaignService.Add(rateCampaign);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(new { message = "Failed", success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, success = false });
            }
        }

        [HttpDelete]
        [Route("RemoveByDonorId/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "donor")]
        public async Task<IActionResult> RemoveByDonorId([FromRoute] int campaignId)
        {
            try
            {
                var result = await _rateCampaignService.RemoveByDonorId(campaignId);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
