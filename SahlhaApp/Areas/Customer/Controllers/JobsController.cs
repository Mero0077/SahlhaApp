using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

using System.Security.Claims;

using SahlhaApp.Utility.NotifcationService;
using SahlhaApp.Utility.NotifcationService.NotificationEvents;
using System.Diagnostics.CodeAnalysis;


namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobService _jobService;

        private readonly JobPostedNotificationHandler _notificationHandler;
        public JobsController(IUnitOfWork unitOfWork, JobService jobService, JobPostedNotificationHandler jobPostedNotificationHandler)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
            _notificationHandler = jobPostedNotificationHandler;
            _notificationHandler.Subscribe(_jobService);
        }

        [HttpPost("PostJob")]
        public async Task<IActionResult> PostJob([FromBody] PostJobRequest postJobRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("User ID not found.");

            var userId = userIdClaim.Value;

            var job = new Job
            {
                //Name=postJobRequest.Name,
                SubServiceId=postJobRequest.SubServiceId,
                Description = postJobRequest.Description,
                Latitude = postJobRequest.Latitude,
                Longitude = postJobRequest.Longitude,
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = userId,
                JobStatus = JobStatus.Pending
            };

          var subService = await _unitOfWork.SubService.GetOne(s => s.Id == job.SubServiceId);

            //    Duration = postJobRequest.Duration,
            //    ApplicationUserId = postJobRequest.ApplicationUserId
            //}; 
            var addedJob = await _jobService.AddJobAsync(job);

            var response = new
            {
                Message = "Job Posted successfully!",
                Data = addedJob
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

    }
}
