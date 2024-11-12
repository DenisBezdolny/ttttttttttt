using Payment.Domain.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Identity
{
    public class User : Entity<Guid>
    {
        //public Guid? Id { get; set; } // Guid 
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string Сountry { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }


        public virtual IEnumerable<Basket> Basket { get; set; } = new List<Basket>();
    }
}
