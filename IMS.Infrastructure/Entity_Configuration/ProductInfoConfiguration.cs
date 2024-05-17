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
    public class ProductInfoConfiguration : IEntityTypeConfiguration<ProductInfo>
    {
        public void Configure(EntityTypeBuilder<ProductInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.ProductName).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.ProductDescription).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.Quantity).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.SoldQuantity).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.RemainingQuantity).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.BatchNo).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.PurchasedDate).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.ExpiryDate).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.ImageUrl).HasMaxLength(50).IsUnicode(true);


            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.ProductInfos)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasOne(e => e.UnitInfo)
              .WithMany(e => e.ProductInfos)
              .HasForeignKey(e => e.UnitInfoId);

            builder.HasOne(e => e.CategoryInfo)
              .WithMany(e => e.ProductInfos)
              .HasForeignKey(e => e.CategoryInfoId);

            builder.HasOne(e => e.SupplierInfo)
              .WithMany(e => e.ProductInfos)
              .HasForeignKey(e => e.SupplierInfoId);

            builder.HasOne(e => e.RackInfo)
             .WithMany(e => e.ProductInfos)
             .HasForeignKey(e => e.RackInfoId);

            builder.HasMany(e => e.ProductRateInfos)
                .WithOne(e => e.ProductInfo)
                .HasForeignKey(e => e.ProductInfoId);

            builder.HasMany(e => e.TransactionInfos)
                .WithOne(e => e.ProductInfo)
                .HasForeignKey(e => e.ProductInfoId);


        }
    }
}
