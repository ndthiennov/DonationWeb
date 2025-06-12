using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;


        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        [Route("GetListByAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> GetListByAdmin(int pageIndex)
        {
            try
            {
                var result = await _campaignService.GetListByAdmin(pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSearchedListByAdmin/{pageIndex}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> GetSearchedListByAdmin([FromRoute] int pageIndex, [FromBody] CampaignSearchADto search)
        {
            try
            {
                var result = await _campaignService.GetSearchedListByAdmin(pageIndex, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSearchedListByUser/{pageIndex}")]
        public async Task<IActionResult> GetSearchedListByUser([FromRoute] int pageIndex, [FromBody] CampaignSearchADto search)
        {
            try
            {
                var result = await _campaignService.GetSearchedListByUser(pageIndex, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSearchedListByOrganiser/{pageIndex}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> GetSearchedListByOrganiser([FromRoute] int pageIndex, [FromBody] CampaignSearchADto search)
        {
            try
            {
                var result = await _campaignService.GetSearchedListByOrganiser(pageIndex, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSearchedListByRecipient/{pageIndex}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recipient")]
        public async Task<IActionResult> GetSearchedListByRecipient([FromRoute] int pageIndex, [FromBody] CampaignSearchADto search)
        {
            try
            {
                var result = await _campaignService.GetSearchedListByRecipient(pageIndex, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{campaignId}")]
        public async Task<IActionResult> GetById([FromRoute] int campaignId)
        {
            try
            {
                var result = await _campaignService.GetById(campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> Add([FromForm] CampaignCUDto campaignCUDto)
        {
            try
            {
                var result = await _campaignService.Add(campaignCUDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> Update([FromRoute] int campaignId, [FromForm] CampaignCUDto campaignCUDto)
        {
            try
            {
                var result = await _campaignService.Update(campaignId, campaignCUDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateDisabledCampaign/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UpdateDisabledCampaign([FromRoute] int campaignId, [FromBody] bool disabled)
        {
            try
            {
                var result = await _campaignService.UpdateDisabledCampaign(campaignId, disabled);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRecivedByRecipient/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recipient")]
        public async Task<IActionResult> UpdateRecivedByRecipient([FromRoute] int campaignId, [FromBody] bool received)
        {
            try
            {
                var result = await _campaignService.UpdateRecivedByRecipient(campaignId, received);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRatedByRecipient/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recipient")]
        public async Task<IActionResult> UpdateRatedByRecipient([FromRoute] int campaignId, [FromBody] RateCampaign rateCampaign)
        {
            try
            {
                var result = await _campaignService.UpdateRatedByRecipient(campaignId, rateCampaign);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API Export to file Excell
        [HttpGet("ExportDonationsToExcel")]
        public IActionResult ExportDonationsToExcel(int campaignId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var excelFile = _campaignService.GenerateExcelReportDonations(campaignId,startDate, endDate);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Donations.xlsx");
        }
        [HttpGet("ExportExpenseToExcel")]
        public IActionResult ExportExpenseToExcel(int campaignId,[FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var excelFile = _campaignService.GenerateExcelReportExpense(campaignId,startDate, endDate);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Expenses.xlsx");
        }



        /*private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        //CAMPAIGN
        [HttpGet]
        [Route("Get/{campaignId}")]
        public async Task<IActionResult> Get([FromRoute] int campaignId)
        {
            try
            {
                var result = await _campaignService.Get(campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateCampaign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> AddOrganiserAccount([FromBody] CampaignDto campaignDto)
        {
            try
            {
                var result = await _campaignService.CreateCampaign(campaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> Delete([FromBody] CampaignDto campaignDto)
        {
            try
            {
                var result = await _campaignService.DeleteCampaign(campaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateCampaign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> UpdateCampaign([FromBody] CampaignDto campaignDto)
        {
            try
            {
                var result = await _campaignService.UpdateCampaign(campaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] int campaignId, int statusId)
        {
            try
            {
                var result = await _campaignService.ChangeStatusCampaign(campaignId, statusId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //RATE CAMPAIGN
        [HttpPost]
        [Route("AddRateCampaign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<IActionResult> AddRateCampaign([FromBody] RateCampaignDto rateCampaignDto)
        {
            try
            {
                var result = await _campaignService.RateCampaign(rateCampaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRateCampaign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<IActionResult> UpdateRateCampaign([FromBody] RateCampaignDto rateCampaignDto)
        {
            try
            {
                var result = await _campaignService.UpdateRateCampaign(rateCampaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //IMAGECAMPAIGN
        [HttpPost]
        [Route("AddImagesCampaign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> AddImagesCampaign([FromBody] List<ImageCampaignDto> listImageCampaignDto)
        {
            try
            {
                var result = await _campaignService.AddListImageCampaign(listImageCampaignDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteListImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> DeleteListImage([FromBody] List<ImageCampaignDto> imageCampaignDtos)
        {
            try
            {
                var result = await _campaignService.RemoveListImageCampaign(imageCampaignDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}