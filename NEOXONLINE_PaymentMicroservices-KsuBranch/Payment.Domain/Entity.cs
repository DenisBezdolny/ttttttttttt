using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public class Entity <TIdentifier> : IEntity
    {
        public TIdentifier? Id {  get; set; }
    }
}
