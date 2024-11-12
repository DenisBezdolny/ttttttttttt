

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payment.Domain;
using Payment.Domain.PayProduct;

namespace Payment.BLL.Contracts.PayProduct
{
    public interface IProductService : IService
    {
        Task<List<Product>> GetProductsFromPriceToPrice(decimal fromAmmount, decimal toAmmount);
    }
}
