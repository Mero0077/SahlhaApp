using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Request;
using System.Security.Claims;

namespace SahlhaApp.Areas.Controllers.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("RateUser")]
        public async Task<IActionResult> RateUserAsync([FromBody] RateUserRequest rateUserRequest)
        {
            if (rateUserRequest.RateValue < 0.5 || rateUserRequest.RateValue > 5)
                return BadRequest("Rate value must be between 1 and 5");

            if (string.IsNullOrEmpty(rateUserRequest.RatedUserId))
                return BadRequest("User to rate must be specified");

            var raterId = User.FindFirstValue(ClaimTypes.NameIdentifier); // from jwt
            if (raterId == null) return Unauthorized("User not authenticated");

            // Check for existing rating
            bool alreadyRated = await _unitOfWork.Rate.Exists(e =>
                e.ApplicationUserId == raterId && e.Provider.ApplicationUserId == rateUserRequest.RatedUserId ||
                e.Provider.ApplicationUserId == raterId && e.ApplicationUserId == rateUserRequest.RatedUserId);

            if (alreadyRated)
                return BadRequest("You have already rated this user.");

            // Find completed assignment
            var assignment = await _unitOfWork.TaskAssignment
                .GetAll(includes: [a => a.Job, a => a.Provider])
                .Where(a => a.IsAccepted && a.Job.JobStatus == JobStatus.Completed)
                .FirstOrDefaultAsync(a =>
                    a.Provider.ApplicationUserId == rateUserRequest.RatedUserId && a.Job.ApplicationUserId == raterId ||
                    a.Provider.ApplicationUserId == raterId && a.Job.ApplicationUserId == rateUserRequest.RatedUserId);

            if (assignment == null)
                return BadRequest("No completed task found between you and this user.");

            var rate = new Rate
            {
                RateValue = rateUserRequest.RateValue,
                Comment = rateUserRequest.Comment,
                CreatedAt = DateTime.UtcNow,
            };

            if (assignment.Job.ApplicationUserId == raterId)
            {
                // Customer rating provider
                rate.ApplicationUserId = raterId;
                rate.ProviderId = assignment.ProviderId;
            }
            else
            {
                // Provider rating customer
                rate.ApplicationUserId = assignment.Job.ApplicationUserId;
                rate.ProviderId = assignment.ProviderId;
            }

            await _unitOfWork.Rate.Add(rate);

            return Ok("Rating submitted successfully.");
        }
    }
}
