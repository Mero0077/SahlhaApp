using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SahlhaApp.Utility.NotifcationService;
namespace SahlhaApp.Areas.Customer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class NotificationTestController : ControllerBase
    {
        private readonly IHubContext<JobHub> _hubContext;

        public NotificationTestController(IHubContext<JobHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public class UserIdDto
        {
            public string UserId { get; set; }
        }

        [HttpPost("")]
        public async Task<IActionResult> SendTestNotification([FromBody] UserIdDto dto)
        {
            await _hubContext.Clients.User(dto.UserId).SendAsync("ReceiveJobNotification", new
            {
                JobId = 1,
                Name = "Test Job",
                Description = "This is a test job sent manually.",
                SubServiceId = 999,
                CreatedAt = DateTime.UtcNow
            });

            return Ok($"✅ Sent notification to userId: {dto.UserId}");
        }
    }
}