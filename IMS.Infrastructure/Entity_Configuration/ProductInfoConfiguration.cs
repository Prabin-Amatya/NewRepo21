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
            builder.Property(e => e.ProductDescription).IsUnicode(true);
            builder.Property(e => e.ImageUrl).IsUnicode(true);


            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
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

         

            builder.HasMany(e => e.ProductRateInfos)
                .WithOne(e => e.ProductInfo)
                .HasForeignKey(e => e.ProductInfoId);

            builder.HasMany(e => e.StockInfos)
                .WithOne(e => e.ProductInfo)
                .HasForeignKey(e => e.ProductInfoId);

            builder.HasMany(e => e.TransactionInfos)
                .WithOne(e => e.ProductInfo)
                .HasForeignKey(e => e.ProductInfoId);


        }
    }
}
