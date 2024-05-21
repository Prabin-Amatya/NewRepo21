using IMS.Modes.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Entity_Configuration
{
    public class CustomerInfoConfiguration : IEntityTypeConfiguration<CustomerInfo>
    {
        public void Configure(EntityTypeBuilder<CustomerInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.CustomerName).HasMaxLength(200).IsUnicode(true).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.PhoneNumber).HasMaxLength(50).IsUnicode(true).IsRequired();

            builder.Property(e => e.Address).HasMaxLength(50).IsUnicode(true).IsRequired();

            builder.Property(e => e.PanNo).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(e => e.CreatedBy).IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.CustomerInfos)
                .HasForeignKey(e => e.StoreInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ProductInvoiceInfos)
                .WithOne(e => e.CustomerInfo)
                .HasForeignKey(e => e.CustomerInfoId);

          
        }
    }
}
