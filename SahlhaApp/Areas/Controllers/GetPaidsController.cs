using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Response.TaskPaid;
using System.Security.Claims;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetPaidsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetPaidsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPaids()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value)) return Unauthorized("User ID not found.");

            var userId = userIdClaim.Value;

            var userJobs = await _unitOfWork.Job
                .GetAll(filter: e => e.ApplicationUserId == userId && e.JobStatus == JobStatus.Pending)
                .ToListAsync();

            if (!userJobs.Any()) return Ok(new List<TaskPaidResponseDto>());

            var jobIds = userJobs.Select(j => j.Id).ToList();

            var taskBids = await _unitOfWork.TaskBid
                .GetAll(
                    filter: x => jobIds.Contains(x.JobId),
                    includes: [e => e.Provider.ApplicationUser]
                )
                .ToListAsync();

            var result = taskBids.Select(bid =>
            {
                var job = userJobs.FirstOrDefault(j => j.Id == bid.JobId);
                var provider = bid.Provider;
                var providerUser = provider?.ApplicationUser;

                return new TaskPaidResponseDto
                {
                    ImageUrl = providerUser?.ImgUrl ?? string.Empty,
                    FirstName = providerUser.FirstName ?? "Unknown",
                    LastName = providerUser?.LastName ?? "",
                    Rate = provider?.VerificationLevel ?? "",
                    Price = bid.Amount
                };
            }).ToList();

            return Ok(result);
        }
    }
}
