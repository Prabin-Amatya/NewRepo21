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
            builder.Property(e => e.InvoiceNo).IsUnicode(true);
            builder.Property(e => e.TransactionDate).IsUnicode(true).HasColumnType("datetime");
            builder.Property(e => e.NetAmount).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.DiscountAmount).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.TotalAmount).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.CancellationRemarks).IsUnicode(true);
            builder.Property(e => e.BillStatus).IsUnicode(true);
            builder.Property(e => e.Remarks).IsUnicode(true);

            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired(false);
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
