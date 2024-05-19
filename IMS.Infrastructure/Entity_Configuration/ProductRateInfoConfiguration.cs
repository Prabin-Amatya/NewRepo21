using IMS.Modes.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Entity_Configuration
{
    public class ProductRateInfoConfiguration : IEntityTypeConfiguration<ProductRateInfo>
    {
        public void Configure(EntityTypeBuilder<ProductRateInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.CostPrice).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.RemainingQuantity).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.BatchNo).IsUnicode(true);
            builder.Property(e => e.SellingPrice).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.Quantity).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.SoldQuantity).IsUnicode(true).HasColumnType("float");
            builder.Property(e => e.PurchasedDate).IsUnicode(true).HasColumnType("datetime");
            builder.Property(e => e.ExpiryDate).IsUnicode(true).HasColumnType("datetime");

            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.CategoryInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.CategoryInfoId);

            builder.HasOne(e => e.RackInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.RackInfoId);

            builder.HasOne(e => e.ProductInfo)
               .WithMany(e => e.ProductRateInfos)
               .HasForeignKey(e => e.ProductInfoId);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasOne(e => e.SupplierInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.SupplierInfoId);

            builder.HasMany(e => e.ProductInvoiceDetailInfos)
                .WithOne(e => e.ProductRateInfo)
                .HasForeignKey(e => e.ProductRateInfoId);

            builder.HasMany(e => e.TransactionInfos)
               .WithOne(e => e.ProductRateInfo)
               .HasForeignKey(e => e.ProductRateInfoId);

            builder.HasMany(e => e.StockInfos)
              .WithOne(e => e.ProductRateInfo)
              .HasForeignKey(e => e.ProductRateInfoId);

        }
    }
}
