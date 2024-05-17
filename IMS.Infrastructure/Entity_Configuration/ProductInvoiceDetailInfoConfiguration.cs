
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
    public class ProductInvoiceDetailInfoConfiguration : IEntityTypeConfiguration<ProductInvoiceDetailInfo>
    {
        public void Configure(EntityTypeBuilder<ProductInvoiceDetailInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Rate).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.Quantity).HasMaxLength(50).IsUnicode(true);


            builder.Property(e => e.Amount).HasMaxLength(50).IsUnicode(true);
          
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);

            builder.HasOne(e => e.ProductInvoiceInfo)
                .WithMany(e => e.ProductInvoiceDetailInfos)
                .HasForeignKey(e => e.ProductInvoiceInfoId);
            builder.HasOne(e => e.ProductRateInfo)
                .WithMany(e => e.ProductInvoiceDetailInfos)
                .HasForeignKey(e => e.ProductRateInfoId);
            



        }
    }
}