using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public class StripeTransaction : Entity<int>
    {
        public string StripeSessionId { get; set; }
        public string PaymentIntentId { get; set; }
        public string PaymentStatus { get; set; }
        public string Currency { get; set; }
        public long Amount { get; set; }
        public string? CustomerId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? InvoiceId { get; set; }
        public string? StatusReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ClientIp { get; set; }//??
    }
}
