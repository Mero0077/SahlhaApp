using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Request;
using System.Security.Claims;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddScheduledTasksController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;

        public AddScheduledTasksController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduledTask([FromRoute] int id)
        {
            var task = await _UnitOfWork.ScheduledTask.GetOne(e => e.Id == id);
            if (task == null) return NotFound($"Task with ID {id} not found.");

            return Ok(task);
        }


        [HttpPost("AddScheduledTask")]
        public async Task<IActionResult> AddScheduledTask([FromBody] PostScheduledTaskRequest postScheduledTaskRequest)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // from jwt  
            var user = await _UserManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            var scheduledTask = postScheduledTaskRequest.Adapt<ScheduledTask>();
            scheduledTask.ApplicationUserId = UserId;

            await _UnitOfWork.ScheduledTask.Add(scheduledTask);

            return CreatedAtAction(nameof(GetScheduledTask), new { id = scheduledTask.Id }, scheduledTask);
        }
    }
}
