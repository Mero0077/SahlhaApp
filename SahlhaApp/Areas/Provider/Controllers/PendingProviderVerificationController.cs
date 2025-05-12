using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.DataAccess.Repositories;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs.Request;
using SahlhaApp.Models.DTOs.Request.Provider;
using SahlhaApp.Models.Models;

namespace SahlhaApp.Areas.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PendingProviderVerificationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public PendingProviderVerificationController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost("JoinAsProvider")]
        public async Task<IActionResult> JoinAsProvider()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound("User not found");

            //providerRequestDto.ApplicationUserId = user.Id;
            //var provider = providerRequestDto.Adapt<SahlhaApp.Models.Models.PendingProviderVerification>();

            await _unitOfWork.PendingProviderVerification.Add(new()
            {
                appliedAt  = DateTime.Now,
                VerificationStatus  = VerificationStatus.Pending,
                ApplicationUserId = user.Id
            });
            await _unitOfWork.PendingProviderVerification.Commit();

            return Ok();
        }
    }
}   
