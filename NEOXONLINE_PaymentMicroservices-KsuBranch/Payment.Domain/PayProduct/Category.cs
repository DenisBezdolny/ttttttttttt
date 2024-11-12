using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.PayProduct
{
    public class Category : Entity<int>
    {
        
        public string Name { get; set; }
        public virtual IEnumerable<Product>? Products { get; set; } = new List<Product>();
    }
}
