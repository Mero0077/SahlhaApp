using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Response.Services;

namespace SahlhaApp.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("")]
        public async Task<IActionResult> Services()
        {
            var subServices= _unitOfWork.SubService.GetAll();
            var services = _unitOfWork.Service
                .GetAll(e => e.Status == true, includes: [e=>e.SubServices])
                .ToList()
                .Adapt<List<ServiceResponseDto>>();

            return Ok(services);
        }

    }
}
