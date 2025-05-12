using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;
using System.Security.Claims;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostJob([FromBody] PostJobRequest postJobRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("User ID not found.");

            var userId = userIdClaim.Value;

            var job = new Job
            {
                Description = postJobRequest.Description,
                Latitude = postJobRequest.Latitude,
                Longitude = postJobRequest.Longitude,
                CreatedAt = DateTime.UtcNow,
                SubServiceId = postJobRequest.SubServiceId,
                ApplicationUserId = userId,
                JobStatus = JobStatus.Pending
            };

            await _unitOfWork.Job.Add(job);

            // تحميل اسم SubService
            var subService = await _unitOfWork.SubService.GetOne(s => s.Id == job.SubServiceId);

            // تجهيز كائن للعرض
            var jobResponse = new
            {
                job.Id,
                job.Description,
                job.Latitude,
                job.Longitude,
                job.CreatedAt,
                job.JobStatus,
                SubServiceName = subService?.Name
            };

            return Ok(jobResponse);
        }

    }
}
