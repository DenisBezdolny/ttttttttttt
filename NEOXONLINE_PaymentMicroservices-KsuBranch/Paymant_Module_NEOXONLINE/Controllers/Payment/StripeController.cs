using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.Application.Payment_DAL.Contracts;
using Payment.BLL.Contracts.Payment;
using Payment.BLL.DTOs;
using Payment.Domain;
using Payment.Domain.ECommerce;
using Payment.Domain.PayProduct;
using Stripe;
using Stripe.Checkout;
using Stripe.FinancialConnections;
using Stripe.Terminal;
using Stripe.V2;
using static Paymant_Module_NEOXONLINE.Controllers.CurrancyLayer.CurrencyLayerController;

namespace Paymant_Module_NEOXONLINE.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly string _webhookSecret;
        private readonly IUnitOfWork _unitOfWork;

        public StripeController(IStripeService stripeService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _stripeService = stripeService;
            _webhookSecret = configuration["Stripe:WebhookSecret"];
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllStripeProducts")]
        public async Task<IActionResult> GetAllStripeProducts()
        {
            try
            {
                var products = await _stripeService.GetAllStripeProductsAsync();
                if (products != null)
                {
                    return Ok(products);
                }
                return NotFound("there aren't any products");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetStripeProduct")]
        public async Task<IActionResult> GetStripeProduct(string id)
        {
            try
            {
                var product = await _stripeService.GetStripeProductAsync(id);
                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound($"product with id {id} does not exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductCreationDto productDto)
        {
            try
            {
                var productId = await _stripeService.CreateStripeProductAsync(productDto);
                var priceId = await _stripeService.CreateStripePriceAsync(productId, productDto.Price);
                return Ok(new { ProductId = productId, PriceId = priceId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateCheckoutSession")]
        public async Task<IActionResult> CreateCheckoutSession(List<string> productIds, string customerId)
        {
            try
            {
                List<string> prices = new List<string>();
                foreach (var productId in productIds)
                {
                    var price = await _stripeService.GetStripePriceIdByProductIdAsync(productId);
                    if (price != null)
                    {
                        prices.Add(price);
                    }
                    else
                    {
                        return NotFound($"price for product {productId} not found");
                    }
                }

                return Ok(await _stripeService.CreateCheckoutSessionAsync(prices, customerId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateStripeCustomer")]
        public async Task<IActionResult> CreateStripeCustomer1(UserDto userDto)
        {
            try
            {
                return Ok(_stripeService.CreateStripeCustomer(userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(string id, ProductCreationDto productDto)
        {
            try
            {
                return Ok(await _stripeService.UpdateStripeProductAsync(id, productDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("ActivateProduct")]
        public async Task<IActionResult> ActivateProduct(string id)
        {
            try
            {
                return Ok(await _stripeService.ActivateStripeProductAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("ArchiveProduct")]
        public async Task<IActionResult> ArchiveProduct(string productId)
        {
            try
            {
                return Ok(await _stripeService.ArchiveStripeProductAsync(productId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            try
            {
                return Ok(await _stripeService.DeleteStripeProductAsync(productId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateRefund")]
        public async Task<IActionResult> CreateRefund(string paymentIntentId, long amount, string reason)
        {
            try
            {
                return Ok(await _stripeService.CreateRefundAsync(paymentIntentId, amount, reason));
            }
            catch (StripeException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("donate")]
        public async Task<IActionResult> Donate(decimal amount, string currency, string customerId)
        {
            if (amount <= 0)
            {
                return BadRequest("Donation amount must be greater than zero.");
            }

            var secret = _stripeService.CreateDonationAsync(amount, currency, customerId);

            return Ok(new { clientSecret = secret });
        }

        [HttpPost("StripeWebhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret,
                    300,
                    false
                );

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var transaction = new StripeTransaction
                    {
                        StripeSessionId = session?.Id,
                        PaymentIntentId = session?.PaymentIntentId,
                        PaymentMethod = session?.PaymentMethodTypes[0],
                        PaymentStatus = session?.PaymentStatus ?? "unknown",
                        CreatedAt = DateTime.UtcNow,
                        Currency = session?.Currency,
                        Amount = session?.AmountTotal ?? 0,
                        CustomerId = session?.CustomerId,
                        ClientIp = HttpContext.Connection.RemoteIpAddress.ToString() //??
                    };

                    if (session?.PaymentStatus == "paid")
                    {
                        Console.WriteLine($"Checkout completed for session: {session.Id}");
                    }
                    else
                    {
                        Console.WriteLine($"Checkout for session: {session.Id} complited with status {session.PaymentStatus}");
                    }

                    _unitOfWork.GetRepository<StripeTransaction>().Create(transaction);
                    await _unitOfWork.SaveShangesAsync();
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var transaction = new StripeTransaction
                    {
                        PaymentIntentId = paymentIntent?.Id,
                        PaymentStatus = "failed",
                        Amount = paymentIntent?.Amount ?? 0,
                        Currency = paymentIntent?.Currency,
                        CustomerId = paymentIntent?.CustomerId,
                        StatusReason = paymentIntent?.LastPaymentError?.Message,
                        CreatedAt = DateTime.UtcNow
                    };

                    _unitOfWork.GetRepository<StripeTransaction>().Create(transaction);
                    await _unitOfWork.SaveShangesAsync();

                    Console.WriteLine($"Payment failed for PaymentIntent: {paymentIntent?.Id}");
                }
                else if (stripeEvent.Type == EventTypes.ChargeRefunded)
                {
                    var charge = stripeEvent.Data.Object as Charge;
                    Console.WriteLine($"Charge refunded: {charge.Id}");
                    //todo add refund info to db 
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (paymentIntent.Metadata.TryGetValue("TransactionType", out var transactionType) && transactionType == "Donation")
                    {
                        var donation = new StripeDonation
                        {
                            PaymentIntentId = paymentIntent.Id,
                            Amount = paymentIntent.Amount / 100m,
                            CustomerId = paymentIntent.CustomerId,
                            Currency = paymentIntent.Currency,
                            CreatedAt = DateTime.UtcNow,
                            IsSuccessful = true
                        };
                        _unitOfWork.GetRepository<StripeDonation>().Create(donation);
                        await _unitOfWork.SaveShangesAsync();

                        Console.WriteLine("Donation completed.");
                    }
                    else
                    {
                        // Логика для других типов транзакций
                        Console.WriteLine("Non-donation payment completed.");
                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"error: {e.Message}");
                return BadRequest();
            }
        }
    }
}
