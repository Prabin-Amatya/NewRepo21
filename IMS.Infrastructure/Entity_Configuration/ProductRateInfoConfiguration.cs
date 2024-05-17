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
            builder.Property(e => e.CostPrice).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.RemainingQuantity).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.BatchNo).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.SellingPrice).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.Quantity).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.SoldQuantity).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.PurchasedDate).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.ExpiryDate).HasMaxLength(200).IsUnicode(true);

            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.CategoryInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.CategoryInfoId);

            builder.HasOne(e => e.UnitInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.UnitInfoId);

            builder.HasOne(e => e.ProductInfo)
               .WithMany(e => e.ProductRateInfos)
               .HasForeignKey(e => e.ProductInfoId);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.ProductRateInfos)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductInvoiceDetailInfos)
                .WithOne(e => e.ProductRateInfo)
                .HasForeignKey(e => e.ProductRateInfoId);

        }
    }
}
