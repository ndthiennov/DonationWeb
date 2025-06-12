using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Get/{phoneNum}")]
        public async Task<IActionResult> Get([FromRoute] string phoneNum)
        {
            try
            {
                var result = await _accountService.Get(phoneNum);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{phoneNum}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute]string phoneNum)
        {
            try
            {
                var result = await _accountService.Delete(phoneNum);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePersonalAccount")]
        public async Task<IActionResult> DeletePersonalAccount()
        {
            try
            {
                var result = await _accountService.DeletePersonalAccount();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateDisabledAccount/{phoneNum}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UpdateDisabledAccount([FromRoute] string phoneNum, [FromBody] bool disabled)
        {
            try
            {
                var result = await _accountService.UpdateDisabledAccount(phoneNum, disabled);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateDisabledPersonalAccount")]
        public async Task<IActionResult> UpdateDisabledPersonalAccount([FromBody] bool disabled)
        {
            try
            {
                var result = await _accountService.UpdateDisabledPersonalAccount(disabled);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddOrganiserAccount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> AddOrganiserAccount([FromForm] SignUpOrganiserDto signUpOrganiserDto)
        {
            try
            {
                var result = await _accountService.AddOrganiserAccount(signUpOrganiserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddDonorAccount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> AddDonorAccount([FromBody] SignUpDonorDto signUpDonorDto)
        {
            try
            {
                var result = await _accountService.AddDonorAccount(signUpDonorDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAdminAccount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> AddAdminAccount([FromBody] SignUpAdminDto signUpAdminDto)
        {
            try
            {
                var result = await _accountService.AddAdminAccount(signUpAdminDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddRecipientAcccount")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> AddRecipientAcccount([FromBody] SignUpRecipientDto signUpRecipientDto)
        {
            try
            {
                var result = await _accountService.AddRecipientAcccount(signUpRecipientDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUncensorOrganiserAccount/{phoneNum}/{organiserId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteUncensorOrganiserAccount([FromRoute] string phoneNum, [FromRoute] int organiserId)
        {
            try
            {
                var result = await _accountService.DeleteUncensorOrganiserAccount(phoneNum, organiserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
