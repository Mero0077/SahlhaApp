using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DocumentTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetDocumentTypes()
        {
            var documentTypes = _unitOfWork.DocumentType.GetAll();
            return Ok(documentTypes);
        }
    }
}
