using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.DTOs.Request;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace SahlhaApp.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
     
        public PaymentsController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] CheckoutRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var taskAssignment = await _unitOfWork.TaskAssignment.GetOne(e=>e.Id== request.TaskAssignmentId);
            if (taskAssignment == null)
                return NotFound("Task assignment not found");

            if (request.PaymentMethod.Equals("CashOnDelivery", StringComparison.OrdinalIgnoreCase))
            {
                var paymentMethod = await _unitOfWork.PaymentMethod.GetOne(pm => pm.Name == "CashOnDelivery");
                if (paymentMethod == null)
                    return StatusCode(500, "CashOnDelivery payment method not configured");

                var payment = new Payment
                {
                    Amount = request.Amount,
                    PaymentDate = DateTime.Now,
                    Status = PaymentStatus.Pending,
                    TaskAssignmentId = request.TaskAssignmentId,
                    ApplicationUserId = userId,
                    PaymentMethodId = paymentMethod.Id,
                    PaymentMethod = paymentMethod
                };

                await _unitOfWork.Payment.Add(payment);
                await _unitOfWork.Commit();

                return Ok(new { Message = "Order placed with Cash on Delivery", PaymentId = payment.Id });
            }
            else if (request.PaymentMethod.Equals("Stripe", StringComparison.OrdinalIgnoreCase))
            {
                Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
                if (string.IsNullOrEmpty(Stripe.StripeConfiguration.ApiKey))
                    return StatusCode(500, "Stripe API key is missing");

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmountDecimal = request.Amount * 100, // cents
                                Currency = request.Currency ?? "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Payment for Task {request.TaskAssignmentId}"
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7289/api/Payments/Success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7289/api/Payments/Cancel",
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                var paymentMethod = await _unitOfWork.PaymentMethod.GetOne(pm => pm.Name == "Stripe");
                if (paymentMethod == null)
                    return StatusCode(500, "Stripe payment method not configured");

                var paymentRecord = new Payment
                {
                    Amount = request.Amount,
                    SessionId = session.Id,
                    Status = PaymentStatus.InProgress,
                    TaskAssignmentId = request.TaskAssignmentId,
                    ApplicationUserId = userId,
                    PaymentMethodId = paymentMethod.Id,
                    PaymentMethod = paymentMethod,
                    PaymentDate = DateTime.Now,
                    PaymentReference = "12",
                    TransactionId="12"
                };

                await _unitOfWork.Payment.Add(paymentRecord);
                await _unitOfWork.Commit();

                return Ok(new { SessionId = session.Id, Url = session.Url });
            }
            else
            {
                return BadRequest("Unsupported payment method");
            }
        }

        [HttpGet]
        public IActionResult Success(string session_id)
        {
            // Payment success logic here or webhook handling
            return Ok(new { Message = "Payment successful!", SessionId = session_id });
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            return Ok(new { Message = "Payment canceled." });
        }
    }
}