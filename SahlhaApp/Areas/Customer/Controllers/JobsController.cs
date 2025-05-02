using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [HttpPost("PostJob")]
        public async Task<IActionResult> PostJob([FromBody] PostJobRequest postJobRequest)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = new Job()
            {
                Description = postJobRequest.Description,
                Address = postJobRequest.Address,
                CreatedAt = DateTime.UtcNow,
                Duration = postJobRequest.Duration,
                ApplicationUserId = postJobRequest.ApplicationUserId
            };
            await _unitOfWork.Job.Add(job);

            return Ok("Job Posted");

        }
    }
}
