using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptBidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcceptBidsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [HttpPost("AcceptProviderBid")]
        public async Task<IActionResult> AcceptProviderBid([FromBody] AcceptProviderRequest acceptProviderRequest)
        {
            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var bid = await _unitOfWork.TaskBid.GetOne(e => e.Id == acceptProviderRequest.TaskBidId, includes: [e => e.Job]);

            if (bid == null) return NotFound("Bid not found.");

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

