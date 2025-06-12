using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

using System.Security.Claims;

using SahlhaApp.Utility.NotifcationService;
using SahlhaApp.Utility.NotifcationService.NotificationEvents;
using System.Diagnostics.CodeAnalysis;
using Mapster;
using Microsoft.AspNetCore.Identity;


namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobService _jobService;
        private readonly UserManager<ApplicationUser> _UserManager;

        private readonly JobPostedNotificationHandler _notificationHandler;
        public JobsController(IUnitOfWork unitOfWork, JobService jobService, JobPostedNotificationHandler jobPostedNotificationHandler, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
            _notificationHandler = jobPostedNotificationHandler;
            _notificationHandler.Subscribe(_jobService);
            _UserManager = userManager;
        }


        [HttpPost("PostJob")]

        public async Task<IActionResult> PostJob([FromBody] PostJobRequest postJobRequest)
        {
            //hello
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("User ID not found.");

            var userId = userIdClaim.Value;
            var user = await _UserManager.FindByIdAsync(userId);

            var job = new Job
            {
                //Name=postJobRequest.Name,
                SubServiceId = postJobRequest.SubServiceId,
                Description = postJobRequest.Description,
                Latitude = postJobRequest.Latitude,
                Longitude = postJobRequest.Longitude,
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = userId,
                JobStatus = JobStatus.Pending,
                Name = user.FirstName+" "+user.LastName
            };

            try
            {
                var subService = await _unitOfWork.SubService.GetOne(s => s.Id == job.SubServiceId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving SubService: {ex.Message}");
            }
            //    Duration = postJobRequest.Duration,
            //    ApplicationUserId = postJobRequest.ApplicationUserId
            //}; 

            try
            {
                var addedJob = await _jobService.AddJobAsync(job);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding job: {ex.Message}");
            }

            var response = new
            {
                Message = "Job Posted successfully!",
                //Data = addedJob
            };

            return Ok(response);
            //var jobResponse = new
            //{
            //    job.Id,
            //    job.Description,
            //    job.Latitude,
            //    job.Longitude,
            //    job.CreatedAt,
            //    job.JobStatus,
            //    SubServiceName = subService?.Name
            //};

            //return Ok(jobResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CancelTask([FromRoute] int id)
        {

            var TaskInDb = await _unitOfWork.Job.GetOne(e => e.Id == id);

            if (TaskInDb == null) return BadRequest($"Task with Id {id} does not exist");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (TaskInDb.ApplicationUserId != userId) return Unauthorized("You are not authorized to cancel this task.");

            if (TaskInDb.JobStatus == JobStatus.Completed) return BadRequest("Completed tasks cannot be cancelled.");
            if (TaskInDb.JobStatus == JobStatus.Cancelled) return BadRequest("This task is already cancelled.");


            TaskInDb.JobStatus = JobStatus.Cancelled;
            TaskInDb.CancelledAt = DateTime.Now;

            await _unitOfWork.Commit();

            return Ok($"Task with Id {id} canceled succesasfully");

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduledTask([FromRoute] int id)
        {
            var task = await _unitOfWork.ScheduledTask.GetOne(e => e.Id == id);
            if (task == null) return NotFound($"Task with ID {id} not found.");

            return Ok(task);
        }


        [HttpPost("")]
        public async Task<IActionResult> AddScheduledTask([FromBody] PostScheduledTaskRequest postScheduledTaskRequest)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // from jwt  
            var user = await _UserManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (postScheduledTaskRequest.ScheduledFor <= DateTime.Now) return BadRequest("date is incorrect");
            var scheduledTask = postScheduledTaskRequest.Adapt<ScheduledTask>();
            scheduledTask.ApplicationUserId = UserId;

            await _unitOfWork.ScheduledTask.Add(scheduledTask);

            return CreatedAtAction(nameof(GetScheduledTask), new { id = scheduledTask.Id }, scheduledTask);
        }


        [HttpPost]
        public async Task<IActionResult> CancelScheduledTask([FromRoute] int id)
        {

            var TaskInDb = await _unitOfWork.ScheduledTask.GetOne(e => e.Id == id);

            if (TaskInDb == null) return BadRequest($"Task with Id {id} does not exist");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (TaskInDb.ApplicationUserId != userId) return Unauthorized("You are not authorized to cancel this task.");

            if (TaskInDb.JobStatus == JobStatus.Completed) return BadRequest("Completed tasks cannot be cancelled.");
            if (TaskInDb.JobStatus == JobStatus.Cancelled) return BadRequest("This task is already cancelled.");


            TaskInDb.JobStatus = JobStatus.Cancelled;
            TaskInDb.CancelledAt = DateTime.Now;

            await _unitOfWork.Commit();

            return Ok($"Task with Id {id} canceled succesasfully");

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
            assignment.Job.CompletedAt = DateTime.Now;

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
