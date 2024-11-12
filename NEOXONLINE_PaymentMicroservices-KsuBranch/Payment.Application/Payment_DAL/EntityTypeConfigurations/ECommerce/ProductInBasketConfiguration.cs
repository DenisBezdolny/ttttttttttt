using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.ECommerce;
using Payment.Domain.PayProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Payment_DAL.EntityTypeConfigurations.ECommerce
{
    public class ProductInBasketConfiguration : IEntityTypeConfiguration<ProductInBasket>
    {
        public void Configure(EntityTypeBuilder<ProductInBasket> builder)
        {
            builder.HasOne(pib => pib.Product).WithMany(prod => prod.ProductInBasket);
            builder.HasOne(pib => pib.Basket).WithMany(b => b.ProductInBasket);
        }
    }
}
