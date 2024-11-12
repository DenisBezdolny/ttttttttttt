using Microsoft.EntityFrameworkCore;
using Payment.Application.Payment_DAL.Contracts;
using Payment.BLL.Contracts.PayProduct;
using Payment.Domain.PayProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.BLL.Services.PayProduct
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Product>> GetProductsFromPriceToPrice(decimal fromAmmount, decimal toAmmount)
        {
            var productRepo = await _unitOfWork.GetRepository<Product>()
                .AsQueryable()
                .ToListAsync();
            var productsFromPriceToPrice = new List<Product>();

            if (productRepo != null)
            {
                
                foreach (var product in productRepo)
                {
                    
                    if (product.Price >= fromAmmount && product.Price <= toAmmount)
                    {
                        productsFromPriceToPrice.Add(product);
                    }
                }
                return productsFromPriceToPrice;
            }
            else
            {
                throw new Exception("There are no products in this range. Please check that the fields are filled in correctly. Change search parameters if necessary");
            }
        }
    }
}
