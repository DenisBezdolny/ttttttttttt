using Payment.Domain.PayProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.ECommerce
{
    public class ProductInBasket : Entity<int>
    {
        public  required Product Product { get; set; } = null!;
        public  required Basket Basket { get; set; } = null!;
    }
}
