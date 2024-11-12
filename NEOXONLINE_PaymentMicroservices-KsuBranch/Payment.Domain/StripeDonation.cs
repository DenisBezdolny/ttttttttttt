using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public class StripeDonation : Entity<int>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PaymentIntentId { get; set; } 
        public bool IsSuccessful { get; set; }
    }
}
