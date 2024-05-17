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
    public class CategoryInfoConfiguration:IEntityTypeConfiguration<CategoryInfo>
    {
        public void Configure(EntityTypeBuilder<CategoryInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.CategoryName).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.CategoryDescription).HasMaxLength(50).IsUnicode(true);
         
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(e => e.CategoryInfos)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductRateInfos)
                .WithOne(e => e.CategoryInfo)
                .HasForeignKey(e => e.CategoryInfoId);

            builder.HasMany(e => e.ProductInfos)
                .WithOne(e => e.CategoryInfo)
                .HasForeignKey(e => e.CategoryInfoId);

            builder.HasMany(e => e.TransactionInfos)
                .WithOne(e => e.CategoryInfo)
                .HasForeignKey(e => e.CategoryInfoId);


        }
    }
}
