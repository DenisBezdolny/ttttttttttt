using Payment.BLL.DTOs;
using Payment.Domain.DTOs;
using Payment.Domain.ECommerce;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.BLL.Contracts.Payment
{
    public interface IStripeService : IService
    {
        Task<StripeList<Product>> GetAllStripeProductsAsync();
        Task<Product> GetStripeProductAsync(string id);
        Task<string?> GetStripePriceIdByProductIdAsync(string stripeProductId);

        Task<string> CreateStripeProductAsync(ProductCreationDto productDto);
        Task<string> CreateStripePriceAsync(string productId, decimal priceAmount);
        Task<string> CreateCheckoutSessionAsync(List<string> prices, string customerId);
        Customer CreateStripeCustomer(UserDto userDto);
        Task<string> CreateRefundAsync(string paymentIntentId, long amount, string reason);

        Task<bool> DeleteStripeProductAsync(string productId);
        Task<bool> ArchiveStripeProductAsync(string productId);
        Task<bool> ActivateStripeProductAsync(string id);

        Task<Product> UpdateStripeProductAsync(string id, ProductCreationDto productDto);

        // Новые методы для поддержки Google Pay и SEPA платежей
        Task<string> ProcessSepaPaymentAsync(PaymentBasket basket, SepaPaymentRequest sepaRequest);
        Task<string> ProcessGooglePayPaymentAsync(PaymentBasket basket, string googlePayToken);
        Task<string> CreateGooglePayDonationAsync(decimal amount, string currency, string googlePayToken);


    }
}