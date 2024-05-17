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
    public class StockInfoConfiguration : IEntityTypeConfiguration<StockInfo>
    {
        public void Configure(EntityTypeBuilder<StockInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Quantity).HasMaxLength(200).IsUnicode(true);

            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);


            builder.HasOne(e => e.CategoryInfo)
                .WithMany(e => e.StockInfos)
                .HasForeignKey(e => e.CategoryInfoId);

            builder.HasOne(e => e.ProductInfo)
               .WithMany(e => e.StockInfos)
               .HasForeignKey(e => e.ProductInfoId);

            builder.HasOne(e => e.StoreInfo)
               .WithMany(e => e.StockInfos)
               .HasForeignKey(e => e.StoreInfoId);

            builder.HasOne(e => e.ProductRateInfo)
               .WithMany(e => e.StockInfos)
               .HasForeignKey(e => e.ProductRateInfoId);

        }
    }
}
