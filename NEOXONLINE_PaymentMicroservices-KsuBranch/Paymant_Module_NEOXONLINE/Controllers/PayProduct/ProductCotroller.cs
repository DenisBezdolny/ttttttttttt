using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Payment_DAL.Contracts;
using Payment.BLL.Contracts.PayProduct;
using Payment.BLL.DTOs;
using Payment.Domain.ECommerce;
using Payment.Domain.PayProduct;
using Payment.BLL.Services.PayProduct;
using System.Xml.Linq;
using Payment.BLL.Contracts.PayProduct;
using Payment.BLL.Contracts.Payment;

namespace Payment_Module_NEOXONLINE.Controllers.PayProduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCotroller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public ProductCotroller(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(_unitOfWork.GetAllIncluding<Product>(p => p.Category).ToList());
        }

        [HttpGet("GetProductsFromPriceToPrice")]
        public async Task<IActionResult> GetProductsFromPriceToPrice(decimal lowerLimit, decimal higherLimit)
        {
            if (lowerLimit < higherLimit)
            {
                var products = await _productService.GetProductsFromPriceToPrice(lowerLimit, higherLimit);
                if(products.Count != 0)
                {
                    return Ok(products);
                }
                else 
                { 
                    return NotFound($"There are no products in range from {lowerLimit} to {higherLimit}"); 
                }
            }
            else
            {
                return BadRequest($"lower limit must be less than higher limit");
            }
        }

        [HttpGet("GetProductByName")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            var product = _unitOfWork.GetAllIncluding<Product>(p=>p.Category).First();
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"product with name {productName} not found ");
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductCreationDto productCreationDto)
        {
            var category = await _unitOfWork.GetRepository<Category>()
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.Name == productCreationDto.CategoryName);

            if (category != null)
            {
                var newProduct = new Product
                {
                    Name = productCreationDto.Name,
                    Description = productCreationDto.Description,
                    Price = productCreationDto.Price,
                    Category = category,
                    ProductInBasket = new List<ProductInBasket>(),
                    Subscription = new List<Subscription>()
                };
                _unitOfWork.GetRepository<Product>().Create(newProduct);
                await _unitOfWork.SaveShangesAsync();

                return Ok(newProduct);
            }
            else
            {
                return NotFound($"category with name {productCreationDto.CategoryName} not found");
            }
        }

        [HttpPut("UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(ProductCreationDto productDto)
        {
            var product = await _unitOfWork.GetRepository<Product>()
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(p => p.Name.Equals(productDto.Name));
            if (product != null)
            {
                var category = await _unitOfWork.GetRepository<Category>()
                    .AsQueryable()
                    .FirstOrDefaultAsync(c => c.Name.Equals(productDto.CategoryName));
                if (category != null)
                {
                    product.Name = productDto.Name;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                    product.Category = category;

                    _unitOfWork.GetRepository<Product>().Update(product);
                    await _unitOfWork.SaveShangesAsync();

                    return Ok(product);
                }
                else
                {
                    return NotFound($"Category with name {productDto.CategoryName} not found");
                }
            }
            else
            {
                return NotFound($"Product with name {productDto.Name} not found");
            }
        }


        [HttpDelete("DeleteProduct")]

        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var deletedProduct = await _unitOfWork.GetRepository<Product>()
                .AsQueryable()
                .FirstOrDefaultAsync(p => p.Name.Equals(productName));
            if (deletedProduct == null)
            {
                return BadRequest(new { message = $"Invalid source data. Not Found {productName}" });
            }
            else
            {
                _unitOfWork.GetRepository<Product>().Delete(deletedProduct);
                await _unitOfWork.SaveShangesAsync();
                //await _stripeService.DeleteStripeProductAsync(deletedProduct.Id);
                return Ok($"{deletedProduct} has been deleted");
            }
        }
    }
}
