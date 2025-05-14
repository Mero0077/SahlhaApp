using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Request.ContactUs;

namespace SahlhaApp.Areas.Shared.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public ContactUsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public IActionResult ContactUs([FromForm] ContactUsRequest request)
        {
            _unitOfWork.ContactUsMessage.Add(request.Adapt<ContactUsMessage>());
            return Ok(request);
        }
    }
}
