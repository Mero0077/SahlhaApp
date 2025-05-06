using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Utility.NotifcationService;

namespace SahlhaApp.Areas.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddBidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobPostedNotificationHandler _notificationHandler;
        private readonly JobService _jobService;
        public AddBidsController(IUnitOfWork unitOfWork, JobService jobService, JobPostedNotificationHandler jobPostedNotificationHandler)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
            _notificationHandler = jobPostedNotificationHandler;
            _notificationHandler.SubscribeTaskBid(_jobService);
        }


        [HttpPost("AddBid")]
        public async Task<IActionResult> AddBid([FromBody] ProviderAddBidRequest providerAddBidRequest)
        {
            var job = await _unitOfWork.Job.GetOne(e => e.Id == providerAddBidRequest.JobId);
            if (job == null)
                return NotFound("Job not found ya m3rs.");

            if (job.JobStatus == JobStatus.Completed || job.JobStatus == JobStatus.Cancelled)
                return BadRequest("You cannot bid on a completed or cancelled job.");

            bool bidExists = await _unitOfWork.TaskBid.Exists(e => e.JobId == providerAddBidRequest.JobId && e.ProviderId == providerAddBidRequest.ProviderId); // time limit

            if (bidExists)
                return BadRequest("You have already placed a bid for this job.");

            var bid = new TaskBid()
            {
                Description = providerAddBidRequest?.Description,
                Amount = providerAddBidRequest.Amount,
                IsAccepted = false,
                CreatedAt = DateTime.Now,
                Duration = providerAddBidRequest.Duration,
                JobId = providerAddBidRequest.JobId,
                ProviderId = providerAddBidRequest.ProviderId,
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
