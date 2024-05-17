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
    public class ProductInvoiceInfoConfiguration : IEntityTypeConfiguration<ProductInvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<ProductInvoiceInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.PaymentMethod).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.InvoiceNo).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.TransactionDate).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.NetAmount).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.DiscountAmount).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.GrossAmount).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.TotalAmount).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.CancellationRemarks).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.BillStatus).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.Remarks).HasMaxLength(50).IsUnicode(true);

            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.ProductInvoiceInfos)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasOne(e => e.CustomerInfo)
                .WithMany(e => e.ProductInvoiceInfos)
                .HasForeignKey(e => e.CustomerInfoId);

            builder.HasMany(e => e.ProductInvoiceDetailInfos)
                .WithOne(e => e.ProductInvoiceInfo)
                .HasForeignKey(e => e.ProductInvoiceInfoId);
        }
    }
}
