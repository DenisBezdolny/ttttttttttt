using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Payment_DAL.EntityTypeConfigurations.ECommerce
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentBasket>
    {
        public void Configure(EntityTypeBuilder<PaymentBasket> builder)
        {    
            builder.HasOne(pay => pay.Basket)
                .WithOne(b => b.PaymentBasket)
                .HasForeignKey<PaymentBasket>(pay => pay.BasketId);
        }
    }
}
