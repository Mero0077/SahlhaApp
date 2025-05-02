using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

namespace SahlhaApp.Areas.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddBidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBidsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("AddBid")]
        public async Task<IActionResult> AddBidAsync([FromBody] ProviderAddBidRequest providerAddBidRequest)
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

            await _unitOfWork.TaskBid.Add(bid);
            return Ok(bid);

        }
    }
}
