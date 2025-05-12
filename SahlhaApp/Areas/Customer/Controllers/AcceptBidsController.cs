using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Utility.NotifcationService;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptBidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobService _jobService;
        private readonly JobPostedNotificationHandler _notificationHandler;

        public AcceptBidsController(IUnitOfWork unitOfWork, JobService jobService, JobPostedNotificationHandler jobPostedNotificationHandler)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
            _notificationHandler = jobPostedNotificationHandler;
            _notificationHandler.Subscribe(_jobService);
        }




        [HttpPost("AcceptProviderBid")]
        public async Task<IActionResult> AcceptProviderBid([FromBody] AcceptProviderRequest acceptProviderRequest)
        {
            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


            var bid = await _unitOfWork.TaskBid.GetOne(e => e.Id == acceptProviderRequest.TaskBidId, includes: [e => e.Job]);

            if (bid == null) return NotFound("Bid not found.");

            var TaskStatus = await _unitOfWork.Job.GetOne(e => e.Id == bid.JobId && e.ApplicationUserId == acceptProviderRequest.ApplicationUserId);

            if (TaskStatus.JobStatus == JobStatus.Cancelled) return BadRequest("Task is cancelled");

            if (bid.Job.ApplicationUserId != acceptProviderRequest.ApplicationUserId) return Unauthorized("You don't own this job.");

            bool jobAlreadyAssigned = await _unitOfWork.TaskAssignment.Exists(t => t.JobId == bid.JobId);
            if (jobAlreadyAssigned) return BadRequest("This job is already assigned to a provider.");

            bid.IsAccepted = true;

            var RemainingBids = _unitOfWork.TaskBid.GetAll(e => e.JobId == bid.JobId && e.Id != bid.Id);

            foreach (var bedo in RemainingBids)
            {
                bedo.IsAccepted = false;
            }

            var taskassignment = new TaskAssignment()
            {
                JobId = bid.JobId,
                ProviderId = bid.ProviderId,
                FinalPrice = bid.Amount,
                AssignedAt = DateTime.Now,
                IsAccepted = true

            };

            await _unitOfWork.TaskAssignment.Add(taskassignment);
            return Ok("Bid accepted and task assigned successfully.");
        }
    }
}

