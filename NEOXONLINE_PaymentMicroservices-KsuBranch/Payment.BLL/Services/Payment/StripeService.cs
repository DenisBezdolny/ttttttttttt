using Microsoft.AspNetCore.Mvc;
using Payment.BLL.Contracts.Payment;
using Payment.BLL.DTOs;
using Payment.BLL.Services.PayProduct;
using Payment.Domain.Identity;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Payment.BLL.Services.Payment
{
    internal class StripeService : IStripeService
    {
        private readonly Stripe.ProductService _productService;
        private readonly PriceService _priceService;
        private readonly SessionService _sessionService;
        private readonly CustomerService _customerService;
        private readonly RefundService _refundService;

        public StripeService()
        {
            _productService = new Stripe.ProductService();
            _priceService = new PriceService();
            _sessionService = new SessionService();
            _customerService = new CustomerService();
            _refundService = new RefundService();
        }

        public async Task<StripeList<Product>> GetAllStripeProductsAsync()
        {
            return await _productService.ListAsync();
        }

        public async Task<Product> GetStripeProductAsync(string id)
        {
            return await _productService.GetAsync(id);
        }

        public async Task<string> CreateStripeProductAsync(ProductCreationDto productDto)
        {
            var productOptions = new ProductCreateOptions
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Metadata = new Dictionary<string, string>
                {
                    { "CategoryName", productDto.CategoryName }
                },
                Shippable = false
            };
            var product = await _productService.CreateAsync(productOptions);

            return product.Id;
        }

        public async Task<string> CreateStripePriceAsync(string productId, decimal priceAmount)
        {
            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = (long?)(priceAmount * 100),
                Currency = "eur",
                Product = productId,
            };
            var price = await _priceService.CreateAsync(priceOptions);

            return price.Id;
        }

        public async Task<Stripe.Product> UpdateStripeProductAsync(string id, ProductCreationDto productDto)
        {
            var productUpdateOptions = new ProductUpdateOptions 
            { 
                Name = productDto.Name,
                Description =  productDto.Description,
                Metadata = new Dictionary<string, string>
                {
                    { "CategoryName", productDto.CategoryName }
                }
            };
            var product = await _productService.UpdateAsync(id, productUpdateOptions);

            return product;
        }

        public async Task<bool> DeleteStripeProductAsync(string productId)
        {
            try
            {
                DeactivateProductPricesAsync(productId);
                var deletedProduct = await _productService.DeleteAsync(productId);//err
                return (bool)deletedProduct.Deleted;
            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

        private async Task DeactivateProductPricesAsync(string productId)
        {
            var prices = await _priceService.ListAsync(new PriceListOptions
            {
                Product = productId
            });
            foreach (var price in prices.Data)
            {
                if (price.Active)
                {
                    var updateOptions = new PriceUpdateOptions
                    {
                        Active = false
                    };
                    await _priceService.UpdateAsync(price.Id, updateOptions);
                }
            }
        }

        public async Task<bool> ArchiveStripeProductAsync(string id)
        {
            try
            {
                var updateOptions = new ProductUpdateOptions
                {
                    Active = false
                };

                var updatedProduct = await _productService.UpdateAsync(id, updateOptions);
                return !updatedProduct.Active;
            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ActivateStripeProductAsync(string id)
        {
            var updateOptions = new ProductUpdateOptions
            {
                Active = true
            };

            var updatedProduct = await _productService.UpdateAsync(id, updateOptions);
            return updatedProduct.Active;
        }

        public async Task<string> CreateCheckoutSessionAsync(List<string> prices, string customerId)
        {
            var lineItems = prices.Select(product => new SessionLineItemOptions
            {
                Price = product,
                Quantity = 1,
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card"/*, "paypal", "sepa_debit"*/},
                LineItems = lineItems,
                Mode = "payment",
                Customer = customerId,
                //SuccessUrl = "https://your-website.com/success?session_id={CHECKOUT_SESSION_ID}",
                SuccessUrl = "https://www.meme-arsenal.com/memes/a2c78af09e451831566e7e90c4269a5c.jpg",
                CancelUrl = "https://cs14.pikabu.ru/images/previews_comm/2023-10_2/1696889858182579745.jpg",
            };
            Session session = await _sessionService.CreateAsync(options);            
            return session.Url;
        }

        public async Task<string?> GetStripePriceIdByProductIdAsync(string stripeProductId)
        {
            var price = (await _priceService.ListAsync(new PriceListOptions
            {
                Product = stripeProductId
            })).FirstOrDefault();
            if(price != null)
            {
                return price.Id;
            }
            return null;
        }

        public Customer CreateStripeCustomer(UserDto userDto)
        {            
            var customerOptions = new CustomerCreateOptions
            {
                Email = userDto.Email,
                Name = userDto.Name,
                Phone = userDto.PhoneNumber,
                Address = new AddressOptions
                {
                    Line1 = userDto.Address,
                    City = userDto.City,
                    Country = userDto.Сountry
                },                
            };
            var customer = _customerService.Create(customerOptions);

            return customer;
        }

        public async Task<string> CreateRefundAsync(string paymentIntentId, long amount, string reason)
        {
            try
            {
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId,
                    Amount = amount, // Сумма возврата в центах (optional, для частичного возврата)
                    Reason = reason ?? "requested_by_customer" //"duplicate", "fraudulent", "requested_by_customer"
                };

                Refund refund = await _refundService.CreateAsync(refundOptions);
                return refund.Id;
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Error occurred during refund: {ex.Message} \n" +
                    $"Error code: {ex.StripeError?.Code} \n" +
                    $"Error type: {ex.StripeError?.Type}");

                if (ex.StripeError?.Code == "insufficient_funds")
                {
                    Console.WriteLine("Insufficient funds for refund.");
                    // Логика для обработки недостатка средств
                }
                else if (ex.StripeError?.Code == "invalid_request_error")
                {
                    Console.WriteLine("Invalid request. Please check payment ID or other parameters.");
                    // Логика для обработки ошибки с неверными параметрами запроса
                }
                else if (ex.StripeError?.Code == "api_error")
                {
                    Console.WriteLine("Stripe API error occured");
                    // Логика для обработки ошибки в Stripe API
                }
                else if (ex.StripeError?.Code == "card_error")
                {
                    Console.WriteLine("problem with the card occured (for example, expired)");
                    // Логика для обработки ошибки из-за проблем с картой (например, истек срок действия).
                }
                throw ex;
            }
        }
    }
}
