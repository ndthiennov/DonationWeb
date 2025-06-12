using DonationAppDemo.Services;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipientService _recipientService;

        public RecipientController(IRecipientService recipientService)
        {
            _recipientService = recipientService;
        }
        [HttpGet]
        [Route("GetAll/{pageIndex}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageIndex)
        {
            try
            {
                var result = await _recipientService.GetAll(pageIndex);
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
                var result = await _recipientService.GetSearchedList(pageIndex, text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{recipientId}")]
        public async Task<IActionResult> GetById([FromRoute] int recipientId)
        {
            try
            {
                var result = await _recipientService.GetById(recipientId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
