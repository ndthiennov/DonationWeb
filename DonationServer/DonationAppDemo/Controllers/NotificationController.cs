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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("CheckReadLatestNotification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser, donor")]
        public async Task<IActionResult> CheckReadLatestNotification()
        {
            try
            {
                var result = await _notificationService.CheckReadLatestNotification();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{pageIndex}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser, donor")]
        public async Task<IActionResult> Get([FromRoute]int pageIndex)
        {
            try
            {
                var result = await _notificationService.Get(pageIndex);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateRead")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "organiser, donor")]
        public async Task<IActionResult> UpdateRead([FromRoute] int notificationId)
        {
            try
            {
                var result = await _notificationService.UpdateRead(notificationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
