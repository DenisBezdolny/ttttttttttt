using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Payment_DAL.Contracts;
using Payment.Domain.ECommerce;

namespace Paymant_Module_NEOXONLINE.Controllers.ECommerce
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBasketController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public PaymentBasketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("AllPaymentBasket")]

        public async Task<IActionResult> GetAllPaymentBasket()
        {
            var repoPayment = await _unitOfWork.GetAllIncluding<PaymentBasket>(pb => pb.Basket).ToListAsync();

            return Ok(repoPayment);
        }


        [HttpGet("PaymentBasketById")]


        public async Task<IActionResult> GetPaymentBasketById (int id)
        {
            var findPaymentBasket = await _unitOfWork.GetAllIncluding<PaymentBasket>(pib => pib.Basket)
                .FirstOrDefaultAsync(pib => pib.BasketId == id);

            if (findPaymentBasket != null)
            {
                return Ok(findPaymentBasket);
            }
            else
            {
                return BadRequest(new { message = $"Invalid source data. Not Found Payment with {id} Id"});
            }
        }



        [HttpDelete("Payment")]

        public async Task<IActionResult> DeletePayment(string source)
        {
            var findPayment = await _unitOfWork.GetRepository<PaymentBasket>()
                .AsQueryable()
                .FirstOrDefaultAsync(pb => pb.Source == source);

            if (findPayment != null)
            {
                var repoPayment = _unitOfWork.GetRepository<PaymentBasket>();
                repoPayment.Delete(findPayment);
                await _unitOfWork.SaveShangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(new { message = $"Invalid source data. Not Found {source}" });
            }

        }
    }
}
