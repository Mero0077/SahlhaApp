using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SahlhaApp.Models.DTOs.Response.Status;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("")]
        public async Task<IActionResult> Status()
        {
            var providersCount = _unitOfWork.Provider.GetAll().Count();
            var serviceCategoriesCount = _unitOfWork.SubService.GetAll(e => e.Status == true).Count();
            var activeServicesCount = _unitOfWork.Service.GetAll(e => e.Status == true).Count();
            var customerReviewsCount = _unitOfWork.Rate.GetAll().Count();

            return Ok(new StatsResponseDto
            {
                Providers = providersCount,
                ServiceCategories = serviceCategoriesCount,
                ActiveServices = activeServicesCount,
                CustomerReviews = customerReviewsCount
            });
        }

    }
}
