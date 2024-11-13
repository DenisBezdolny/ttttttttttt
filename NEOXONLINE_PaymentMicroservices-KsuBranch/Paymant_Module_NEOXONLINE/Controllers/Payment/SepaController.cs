using Microsoft.AspNetCore.Mvc;
using Payment.BLL.Contracts.Payment;
using Payment.BLL.DTOs;
using Payment.Domain.ECommerce;
using Payment.Application.Payment_DAL.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Paymant_Module_NEOXONLINE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SepaController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SepaController> _logger;

        public SepaController(IStripeService stripeService, IUnitOfWork unitOfWork, ILogger<SepaController> logger)
        {
            _stripeService = stripeService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("sepa")]
        public async Task<IActionResult> ProcessSepaPayment([FromBody] SepaPaymentRequest sepaRequest, [FromQuery] int basketId)
        {
            var basket = _unitOfWork.GetRepository<PaymentBasket>()
                .AsQueryable()
                .FirstOrDefault(pb => pb.Id == basketId);

            if (basket == null)
            {
                return NotFound(new { message = $"Basket with ID {basketId} not found." });
            }

            if (basket.Amount <= 0 || basket.Basket.User == null)
            {
                return BadRequest(new { message = "Invalid basket data or missing user information." });
            }

            var resultMessage = await _stripeService.ProcessSepaPaymentAsync(basket, sepaRequest);

            if (resultMessage.Contains("Payment completed successfully"))
            {
                return Ok(new { success = true, message = resultMessage });
            }
            else if (resultMessage.Contains("processing"))
            {
                return Accepted(new { success = true, message = resultMessage });
            }
            else
            {
                _logger.LogError("Failed to process SEPA payment for basket ID: {BasketId}", basketId);
                return BadRequest(new { success = false, message = resultMessage });
            }
        }
    }
}
