using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.BLL.DTOs
{
    public class UserDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Сountry { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
