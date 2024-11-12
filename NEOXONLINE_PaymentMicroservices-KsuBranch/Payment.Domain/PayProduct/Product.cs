using Payment.Domain.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.PayProduct
{
    public class Product : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Category? Category { get; set; }
        public decimal? Price { get; set; }
        public virtual required IEnumerable<ProductInBasket> ProductInBasket { get; set; }
        public virtual required IEnumerable<Subscription> Subscription { get; set; }
    }
}
