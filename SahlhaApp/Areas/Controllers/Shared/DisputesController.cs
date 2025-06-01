using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request.DisputeRequests;
using System.Security.Claims;

namespace SahlhaApp.Areas.Controllers.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisputesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisputesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("PostDispute")]
        public async Task<IActionResult> PostDispute([FromBody] DisputeRequestDto disputeRequestDto)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null) return BadRequest("User Not Found");

            var IsTaskCompleted = await _unitOfWork.TaskAssignment.GetOne(e => e.Id == disputeRequestDto.TaskAssignmentId);

            if (IsTaskCompleted == null || IsTaskCompleted.IsCompleted == false) return BadRequest("You can't post a dispute at the moment!");

            var disputeExists = await _unitOfWork.Dispute.Exists(e => e.TaskAssignmentId == disputeRequestDto.TaskAssignmentId && e.ApplicationUserId == UserId);

            if (disputeExists) return BadRequest("A dispute already exists for this task assignment!");

            disputeRequestDto.ApplicationUserId = UserId;
            disputeRequestDto.CreatedAt = DateTime.UtcNow;
            disputeRequestDto.Status = false;
            var Dispute = disputeRequestDto.Adapt<Dispute>();
            await _unitOfWork.Dispute.Add(Dispute);
            return Ok(Dispute);
        }
    }
}
