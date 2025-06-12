using Microsoft.AspNetCore.Mvc;
using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.DTOs;
using System.Threading.Tasks;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenceController : ControllerBase
    {
        private readonly ITransferenceService _transferenceService;

        public TransferenceController(ITransferenceService transferenceService)
        {
            _transferenceService = transferenceService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddTransference([FromBody] TransferenceDto transferenceDto)
        {
            try
            {
                await _transferenceService.AddTransference(transferenceDto);
                return Ok("Transference added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateTransference(int id, [FromBody] TransferenceDto transferenceDto)
        {
            try
            {
                await _transferenceService.UpdateTransference(id, transferenceDto);
                return Ok("Transference updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteTransference(int id)
        {
            try
            {
                await _transferenceService.DeleteTransference(id);
                return Ok("Transference deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllTransferences([FromQuery] int campaignId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = await _transferenceService.GetAllTransferences(campaignId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}