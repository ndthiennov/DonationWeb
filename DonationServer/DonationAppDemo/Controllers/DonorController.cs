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
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        [Route("GetAll/{pageIndex}")]
        public async Task<IActionResult> GetAll([FromRoute]int pageIndex)
        {
            try
            {
                var result = await _donorService.GetAll(pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("GetSearchedList/{pageIndex}")]
        public async Task<IActionResult> GetSearchedList([FromRoute] int pageIndex, [FromBody] string text)
        {
            try
            {
                var result = await _donorService.GetSearchedList(pageIndex, text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{donorId}")]
        public async Task<IActionResult> GetById([FromRoute]int donorId)
        {
            try
            {
                var result = await _donorService.GetById(donorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{donorId}")]
        public async Task<IActionResult> Update([FromRoute] int donorId, [FromBody] DonorDto donorDto)
        {
            try
            {
                var result = await _donorService.Update(donorId, donorDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAva/{donorId}")]
        public async Task<IActionResult> UpdateAva([FromRoute] int donorId, IFormFile avaFile)
        {
            try
            {
                var result = await _donorService.UpdateAva(donorId, avaFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("BecomeDonor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,organiser")]
        public async Task<IActionResult> BecomeDonor([FromBody] SignUpDonorDto signUpDonorDto)
        {
            try
            {
                var result = await _donorService.BecomeDonor(signUpDonorDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
