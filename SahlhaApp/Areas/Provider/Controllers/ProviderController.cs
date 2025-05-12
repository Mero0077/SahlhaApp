using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Models.DTOs.Request.Provider;
using SahlhaApp.Models.Models;

namespace SahlhaApp.Areas.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProviderController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost("")]
        public async Task<IActionResult> SelectWorkDays(WorkDaysRequestDto workDaysRequestDto)
        {
            if(workDaysRequestDto is null) return BadRequest("WorkDaysRequestDto cannot be null");

            var providerWorkDays = workDaysRequestDto.WorkDays.Select(workDay => new ProviderServiceAvailability
            {
                Day = workDay,
                ProviderId = workDaysRequestDto.ProviderId
            }).ToList();
            await _unitOfWork.ProviderServiceAvailability.AddAll(providerWorkDays);
            return Ok();    
        }
    }
}
