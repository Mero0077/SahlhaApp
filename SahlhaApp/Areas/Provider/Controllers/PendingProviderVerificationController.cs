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
    [AllowAnonymous]
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
        public async Task<IActionResult> JoinAsProvider([FromForm] ProviderRequestDto providerRequestDto)
        {
            var userexists= _userManager.FindByIdAsync(providerRequestDto.ApplicationUserId);
            if (userexists == null) return Unauthorized();

            var fileMap = await DocumentHelper.HandleProviderDocumentsAsync(providerRequestDto.Id, providerRequestDto.BirthCertificate, providerRequestDto.CriminalRecord);

           
            foreach (var document in fileMap)
            {
                var docType = await _unitOfWork.DocumentType.GetOne(dt => dt.Name == document.Key);

                if (docType == null) return BadRequest($"Invalid document type: {document.Key}");

                var doc = new Document()
                {
                    Name = document.Key,
                    Url=document.Value,
                    UploadedAt = DateTime.Now,
                    ApplicationUserId = providerRequestDto.ApplicationUserId,
                    DocumentTypeId=docType.Id

                };
                await _unitOfWork.Document.Add(doc);
            }
            await _unitOfWork.PendingProviderVerification.Add(new()
            {
                appliedAt  = DateTime.Now,
                VerificationStatus  = VerificationStatus.Pending,
                ApplicationUserId = providerRequestDto.ApplicationUserId
            });

            return Ok("Provider application submitted successfully.");
        }
    }
}   
