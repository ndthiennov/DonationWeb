using Microsoft.AspNetCore.Mvc;
using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.DTOs;
using System.Threading.Tasks;
using DonationAppDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DonationAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        [Route("GetListByCampaign/{campaignId}")]
        public async Task<IActionResult> GetListByCampaign([FromRoute] int campaignId)
        {
            try
            {
                var result = await _expenseService.GetListByCampaign(campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Add/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> Add([FromRoute] int campaignId, [FromBody] ExpenseDto expenseDto)
        {
            try
            {
                var result = await _expenseService.Add(campaignId, expenseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Delete/{expenseId}/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser")]
        public async Task<IActionResult> Delete([FromRoute] int expenseId, [FromRoute] int campaignId, [FromBody] ExpenseDto expenseDto)
        {
            try
            {
                var result = await _expenseService.Delete(expenseId, campaignId, expenseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                await _expenseService.AddExpense(expenseDto);
                return Ok("Expense added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDto expenseDto)
        {
            try
            {
                await _expenseService.UpdateExpense(id, expenseDto);
                return Ok("Expense updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await _expenseService.DeleteExpense(id);
                return Ok("Expense deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllExpenses([FromQuery] int campaignId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = await _expenseService.GetAllExpenses(campaignId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}