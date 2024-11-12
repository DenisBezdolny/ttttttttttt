using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.ECommerce
{
    public class PaymentBasket : Entity<int>
    {
        public required int BasketId { get; set; }
        public virtual required Basket Basket { get; set; }
        public required DateTime Date { get; set; }
        public required decimal Amount { get; set; }
        public required string Source { get; set; }
        public required string MetaData { get; set; }
    }
}
