using Payment.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.ECommerce
{
    public class Basket : Entity<int>
    {
        
        public virtual PaymentBasket? PaymentBasket { get; set; }
        public virtual  User User { get; set; } = null!;
        public virtual  IEnumerable<ProductInBasket> ProductInBasket { get; set; } = new List<ProductInBasket>();
    }
}
