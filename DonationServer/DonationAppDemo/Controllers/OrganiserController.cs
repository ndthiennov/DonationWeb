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
    public class OrganiserController : ControllerBase
    {
        private readonly IOrganiserService _organiserService;

        public OrganiserController(IOrganiserService organiserService)
        {
            _organiserService = organiserService;
        }

        [HttpGet]
        [Route("GetAll/{pageIndex}")]
        public async Task<IActionResult> GetAll([FromRoute]int pageIndex)
        {
            try
            {
                var result = await _organiserService.GetAll(pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("GetSearchedList/{pageIndex}")]
        public async Task<IActionResult> GetSearchedList([FromRoute] int pageIndex, [FromBody]string text)
        {
            try
            {
                var result = await _organiserService.GetSearchedList(pageIndex, text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllUnCensored/{pageIndex}")]
        public async Task<IActionResult> GetAllUnCensored([FromRoute] int pageIndex)
        {
            try
            {
                var result = await _organiserService.GetAllUnCensored(pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("GetSearchedUncensoredList/{pageIndex}")]
        public async Task<IActionResult> GetSearchedUncensoredList([FromRoute] int pageIndex, [FromBody] string text)
        {
            try
            {
                var result = await _organiserService.GetSearchedUncensoredList(pageIndex, text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{organiserId}")]
        public async Task<IActionResult> GetById([FromRoute]int organiserId)
        {
            try
            {
                var result = await _organiserService.GetById(organiserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{organiserId}")]
        public async Task<IActionResult> Update([FromRoute] int organiserId, [FromBody]OrganiserDto organiserDto)
        {
            try
            {
                var result = await _organiserService.Update(organiserId, organiserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAva/{organiserId}")]
        public async Task<IActionResult> UpdateAva([FromRoute] int organiserId, IFormFile avaFile)
        {
            try
            {
                var result = await _organiserService.UpdateAva(organiserId, avaFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateCertification/{organiserId}")]
        public async Task<IActionResult> UpdateCertification([FromRoute] int organiserId, IFormFile certificationFile)
        {
            try
            {
                var result = await _organiserService.UpdateCertification(organiserId, certificationFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("BecomeOrganiser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,donor")]
        public async Task<IActionResult> BecomeOrganiser([FromBody]SignUpOrganiserDto signUpOrganiserDto)
        {
            try
            {
                var result = await _organiserService.BecomeOrganiser(signUpOrganiserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateApprovement/{organiserId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UpdateApprovement([FromRoute] int organiserId)
        {
            try
            {
                var result = await _organiserService.UpdateApprovement(organiserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
