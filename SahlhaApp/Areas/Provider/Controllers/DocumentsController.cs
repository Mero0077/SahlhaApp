using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request;

namespace SahlhaApp.Areas.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public DocumentsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        //[HttpPost("UploadDocuments")]
        //public async Task<IActionResult> UploadDocuments([FromForm] DocumentRequestDto documentRequestDto)
        //{
        //    var user = await _userManager.GetUserAsync(User); if (user is null) return NotFound("User not found");

        //    if (documentRequestDto is null || documentRequestDto.File == null || !documentRequestDto.File.Any()) return BadRequest("No files uploaded.");

        //    var fileNames = await DocumentHelper.HandleMultipleFiles(documentRequestDto.File);

        //    if (fileNames == null || !fileNames.Any()) return StatusCode(500, "Error saving files.");

        //    foreach (var fileName in fileNames)
        //    {
        //        await _unitOfWork.Document.Add(new Document
        //        {
        //            Name = fileName,
        //            Url = fileName,
        //            UploadedAt = DateTime.UtcNow,
        //            Status = Status.Pending,
        //            VerifiedAt = null,
        //            ApplicationUserId = user.Id
        //        });
        //    }

        //    await _unitOfWork.Document.Commit();

        //    return Ok(new { Message = "Files uploaded successfully", Files = fileNames });
        //}


    }
}
