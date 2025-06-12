using DonationAppDemo.DTOs;
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
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        private readonly IDonationHubService _donationHubService;

        public DonationController(IDonationService donationService,
            IDonationHubService donationHubService)
        {
            _donationService = donationService;
            _donationHubService = donationHubService;
        }

        /*[HttpPost]
        [Route("GetListByCampaignId/{campaignId}")]
        public async Task<IActionResult> GetListByCampaignId([FromRoute]int campaignId, [FromBody]SearchDto searchDto)
        {
            try
            {

                var result = await _donationService.GetListByCampaignId(campaignId, searchDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }*/

        [HttpPost]
        [Route("GetSearchedListByCampaignId/{campaignId}")]
        public async Task<IActionResult> GetSearchedListByCampaignId([FromRoute] int campaignId, [FromBody] SearchDto searchDto)
        {
            try
            {

                var result = await _donationService.GetSearchedListByCampaignId(campaignId, searchDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
/*
        [HttpPost]
        [Route("GetListByDonorId")]
        public async Task<IActionResult> GetListByDonorId([FromBody] SearchDto searchDto)
        {
            try
            {

                var result = await _donationService.GetListByDonorId(searchDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }*/

        [HttpPost]
        [Route("CreatePaymentUrl")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "donor")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody]PaymentRequestDto request)
        {
            try
            {
                var result = await _donationService.CreatePaymentUrl(HttpContext, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{PaymentExcecute?}")]
        public async Task<IActionResult> PaymentExcecute()
        {
            try
            {

                var result = await _donationService.PaymentExecute(Request.Query);
                await _donationHubService.SendDonation(result);

                return Redirect($"http://localhost:4200/donation/200");
            }
            catch (Exception ex)
            {
                return Redirect($"http://localhost:4200/donation/400");
            }
        }
    }
}
