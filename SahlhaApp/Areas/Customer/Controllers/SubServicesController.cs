using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SubServicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubServicesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetSubServices()
        {
            var subServices =  _unitOfWork.SubService.GetAll();
            return Ok(subServices);
        }
    }
}
