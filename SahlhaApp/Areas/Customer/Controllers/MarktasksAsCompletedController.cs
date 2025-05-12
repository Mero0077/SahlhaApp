using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarktasksAsCompletedController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarktasksAsCompletedController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("MarkTaskCompleted")]
        public async Task<IActionResult> MarkTaskCompleted(int taskAssignmentId)
        {
            var assignment = await _unitOfWork.TaskAssignment.GetOne(e => e.Id == taskAssignmentId, includes: [e => e.Job]);

            if (assignment == null) return NotFound("Assignment not found");


            if (assignment.IsCompleted)
                return BadRequest("Task already marked as completed");

            if (!assignment.IsAccepted)
                return BadRequest("Task must be accepted first");


            assignment.IsCompleted = true;
            assignment.Job.JobStatus = JobStatus.Completed;
            //assignment.Job.CompletedAt = DateTime.Now;

            // Calculate and handle duration logic
            var actualDuration = DateTime.Now - assignment.AssignedAt;
            var expectedDuration = TimeSpan.FromHours((double)assignment.Job.Duration);

            if (actualDuration > expectedDuration)
            {
                // Handle late completion notifications

            }

            await _unitOfWork.Commit();
            return Ok("Task marked as completed");

        }
    }
}
