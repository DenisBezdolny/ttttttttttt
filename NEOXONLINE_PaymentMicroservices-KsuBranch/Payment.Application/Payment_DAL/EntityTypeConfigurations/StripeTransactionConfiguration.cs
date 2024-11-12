using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Payment_DAL.EntityTypeConfigurations
{
    internal class StripeTransactionConfiguration : IEntityTypeConfiguration<StripeTransaction>
    {
        public void Configure(EntityTypeBuilder<StripeTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.StripeSessionId)
                .IsRequired();

            builder.Property(t => t.PaymentStatus)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();
        }
    }
}
