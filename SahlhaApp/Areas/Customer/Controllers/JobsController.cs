using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Utility.NotifcationService;
using SahlhaApp.Utility.NotifcationService.NotificationEvents;
using System.Diagnostics.CodeAnalysis;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        //[Authorize]
        [HttpPost("PostJob")]
        public async Task<IActionResult> PostJob([FromBody] PostJobRequest postJobRequest)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = new Job()
            {
                Name=postJobRequest.Name,
                SubServiceId=postJobRequest.SubServiceId,
                Description = postJobRequest.Description,
                Address = postJobRequest.Address,
                CreatedAt = DateTime.UtcNow,
                Duration = postJobRequest.Duration,
                ApplicationUserId = postJobRequest.ApplicationUserId
            }; 
            var addedJob = await _jobService.AddJobAsync(job);

            var response = new
            {
                Message = "Job Posted successfully!",
                Data = addedJob
            };

            return Ok(response);

        }
    }
}
