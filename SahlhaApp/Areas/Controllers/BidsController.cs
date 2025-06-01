using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Utility.NotifcationService;
using System.Security.Claims;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobService _jobService;
        private readonly JobPostedNotificationHandler _notificationHandler;

        public BidsController(IUnitOfWork unitOfWork, JobService jobService, JobPostedNotificationHandler jobPostedNotificationHandler)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
            _notificationHandler = jobPostedNotificationHandler;
            _notificationHandler.Subscribe(_jobService);
        }

        [HttpPost("AcceptProviderBid")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> AcceptProviderBid([FromBody] AcceptProviderRequest acceptProviderRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var bid = await _unitOfWork.TaskBid.GetOne(e => e.Id == acceptProviderRequest.TaskBidId, includes: [e => e.Job]);

            if (bid == null) return NotFound("Bid not found.");

            var TaskStatus = await _unitOfWork.Job.GetOne(e => e.Id == bid.JobId && e.ApplicationUserId == userId);

            if (TaskStatus.JobStatus == JobStatus.Cancelled) return BadRequest("Task is cancelled");

            if (bid.Job.ApplicationUserId != userId) return Unauthorized("You don't own this job.");

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

        [HttpPost("AddBid")]
        //[Authorize(Roles ="Provider")]
        public async Task<IActionResult> AddBid([FromBody] ProviderAddBidRequest providerAddBidRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("Invalid token");

            var job = await _unitOfWork.Job.GetOne(e => e.Id == providerAddBidRequest.JobId);
            if (job == null)
                return NotFound("Job not found ya m3rs.");

            if (job.JobStatus == JobStatus.Completed || job.JobStatus == JobStatus.Cancelled)
                return BadRequest("You cannot bid on a completed or cancelled job.");

            var provider = await _unitOfWork.Provider.GetOne(e => e.ApplicationUserId == userId);


            bool bidExists = await _unitOfWork.TaskBid.Exists(e => e.JobId == providerAddBidRequest.JobId && e.ProviderId == provider.Id); // time limit


            if (bidExists)
                return BadRequest("You have already placed a bid for this job.");

            int providerId = provider.Id; // Or however you identify provider
            var bid = new TaskBid()
            {
                Description = providerAddBidRequest?.Description,
                Amount = providerAddBidRequest.Amount,
                IsAccepted = false,
                CreatedAt = DateTime.Now,
                Duration = providerAddBidRequest.Duration,
                JobId = providerAddBidRequest.JobId,
                ProviderId = providerId
            };

            var BidAdded = await _jobService.AddTaskBid(bid);

            var response = new
            {
                Message = "Bid successfully added!",
                Data = BidAdded
            };
            return Ok(response);

        }
    }
}
