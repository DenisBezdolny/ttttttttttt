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
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            /*builder.HasOne<PaymentBasket>().WithOne(pay => pay.Basket);*/
            builder.HasOne(b => b.User).WithMany(b => b.Basket);
            /*builder.HasMany<ProductInBasket>().WithOne(pib => pib.Basket);*/
        }
    }
}
