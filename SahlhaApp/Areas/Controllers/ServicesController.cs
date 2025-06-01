using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Response.Services;
using SahlhaApp.Utility.NotifcationService;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Provider")]
    public class ServicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHubContext<JobHub> _hubContext;
        public ServicesController(IUnitOfWork unitOfWork, IHubContext<JobHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Services()
        {
            var subServices = _unitOfWork.SubService.GetAll();
            var services = _unitOfWork.Service
                .GetAll(e => e.Status == true, includes: [e => e.SubServices])
                .ToList()
                .Adapt<List<ServiceResponseDto>>();

            return Ok(services);
        }

    }
}
