using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Payment_DAL.Contracts;
using Payment.Application.Payment_DAL.RealisationInterfaces;
using Payment.Domain.ECommerce;
using Payment.Domain.PayProduct;

namespace Paymant_Module_NEOXONLINE.Controllers.ECommerce
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInBasketController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductInBasketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("AllProductsInBasket")]

        public async Task<IActionResult> GetAllProductsInBasket()
        {
            var repoProductInBasket = await _unitOfWork.GetAllIncluding<ProductInBasket>(pib => pib.Product)
                .Include(pib => pib.Basket)
                .ThenInclude(b => b.PaymentBasket)
                .ToListAsync();
            return Ok(repoProductInBasket);
        }


        [HttpGet("GetUsersProductsInBasket")]

        public async Task<IActionResult> GetUsersProductsInBasket(string username)
        {
            var repoProductInBasket = await _unitOfWork.GetAllIncluding<ProductInBasket>(pib => pib.Product).Where(pib=>pib.Basket.User.FirstName.Equals(username)).ToListAsync();
            return Ok(repoProductInBasket);
        }

        [HttpPost("CreateProductInBasket")]

        public async Task<IActionResult> CreateProductInBasket(string productName, string username)
        {
            var product = _unitOfWork.GetRepository<Product>()
                .AsQueryable().First(p => p.Name.Equals(productName));
            if(product != null)
            {
                var basket = _unitOfWork.GetAllIncluding<Basket>(b => b.User)
                    .AsQueryable().First(b => b.User.FirstName.Equals(username));
                if (basket != null)
                {
                    var productInBasket = new ProductInBasket()
                    {
                        Basket = basket,
                        Product = product
                    };
                    _unitOfWork.GetRepository<ProductInBasket>().Create(productInBasket);
                    await _unitOfWork.SaveShangesAsync();
                    return Ok(productInBasket);
                }
                else
                {
                    return NotFound($"user with name {username} not found");
                }
            }
            else
            {
                return NotFound($"product with name {productName} not found");
            }
        }


        [HttpDelete("DeleteProductInBasket")]

        public async Task<IActionResult> DeleteProductInBasket (string productName, string username)
        {
            var findProductInBasket = await _unitOfWork.GetRepository<ProductInBasket>()
                .AsQueryable()
                .FirstOrDefaultAsync(pib => pib.Product.Name.Equals(productName) && pib.Basket.User.FirstName.Equals(username));
            if (findProductInBasket != null)
            {

                var repoProductInBasket = _unitOfWork.GetRepository<ProductInBasket>();
                repoProductInBasket.Delete(findProductInBasket);
                await _unitOfWork.SaveShangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(new { message = $"Invalid source data. Not Found Product in basket with {productName} name and {username} username" });
            }           
        }
    }
}
