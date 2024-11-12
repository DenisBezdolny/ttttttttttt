
using Microsoft.AspNetCore.Mvc;
using Payment.BLL.Contracts.Payment;
using Payment.Domain.ECommerce;
using System.Threading.Tasks;

namespace Paymant_Module_NEOXONLINE.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public PaymentController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("googlepay")]
        public async Task<IActionResult> ProcessGooglePayPayment([FromBody] PaymentBasket basket, [FromQuery] string googlePayToken)
        {
            if (basket == null || string.IsNullOrEmpty(googlePayToken))
            {
                return BadRequest("Invalid basket data or Google Pay token.");
            }

            // Вызов метода сервиса для обработки оплаты
            var result = await _stripeService.ProcessGooglePayPaymentAsync(basket, googlePayToken);

            // На основе результата возвращаем соответствующий ответ
            if (result.Contains("completed successfully"))
            {
                return Ok(new { message = result });
            }
            else if (result.Contains("processing"))
            {
                return Accepted(new { message = result });
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }
    }
}
