using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Provider,User")]
    public class SubServicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetSubServices")]
        public async Task<IActionResult> GetSubServices()
        {
            var subServices = _unitOfWork.SubService.GetAll();
            return Ok(subServices);
        }
    }
}
