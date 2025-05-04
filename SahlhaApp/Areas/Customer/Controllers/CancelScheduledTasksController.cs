using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancelScheduledTasksController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public CancelScheduledTasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public async Task<IActionResult> CancelScheduledTask([FromRoute] int  id)
        {
            
            var TaskInDb= await _unitOfWork.ScheduledTask.GetOne(e=>e.Id==id);

            if (TaskInDb == null) return BadRequest($"Task with Id {id} does not exist");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (TaskInDb.ApplicationUserId != userId) return Unauthorized("You are not authorized to cancel this task.");

            if (TaskInDb.JobStatus == JobStatus.Completed) return BadRequest("Completed tasks cannot be cancelled.");
            if(TaskInDb.JobStatus== JobStatus.Cancelled) return BadRequest("This task is already cancelled.");


           TaskInDb.JobStatus=JobStatus.Cancelled;
           TaskInDb.CancelledAt=DateTime.Now;

           await  _unitOfWork.Commit();

          return Ok($"Task with Id {id} canceled succesasfully");
           
        }
    }
}
