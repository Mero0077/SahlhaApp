using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SahlhaApp.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DocumentTypesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetDocumentTypes()
        {
            var documentTypes = _unitOfWork.DocumentType.GetAll();
            return Ok(documentTypes);
        }
    }
}
