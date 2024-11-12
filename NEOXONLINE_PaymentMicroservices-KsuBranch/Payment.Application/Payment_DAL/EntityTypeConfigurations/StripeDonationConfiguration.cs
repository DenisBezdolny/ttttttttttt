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
    internal class StripeDonationConfiguration : IEntityTypeConfiguration<StripeDonation>
    {
        public void Configure(EntityTypeBuilder<StripeDonation> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Amount)
                .IsRequired();

            builder.Property(d => d.Currency)
                .IsRequired();

            builder.Property(d => d.CustomerId)
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.PaymentIntentId)
                .IsRequired();

            builder.Property(d => d.IsSuccessful)
                .IsRequired();
        }
    }
}
