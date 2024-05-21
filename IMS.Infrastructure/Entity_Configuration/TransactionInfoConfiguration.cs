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
    public class TransactionInfoConfiguration : IEntityTypeConfiguration<TransactionInfo>
    {
        public void Configure(EntityTypeBuilder<TransactionInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.TransactionType).HasMaxLength(200).IsUnicode(true).IsRequired();
            builder.Property(e => e.Rate).IsUnicode(true).HasColumnType("float").IsRequired();
            builder.Property(e => e.Amount).IsUnicode(true).HasColumnType("float").IsRequired();
            builder.Property(e => e.Quantity).IsUnicode(true).HasColumnType("float").IsRequired();
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.CategoryInfo)
            .WithMany(e => e.TransactionInfos)
            .HasForeignKey(e => e.CategoryInfoId)
             .OnDelete(DeleteBehavior.Restrict);
            

            builder.HasOne(e => e.ProductInfo)
            .WithMany(e => e.TransactionInfos)
            .HasForeignKey(e => e.ProductInfoId)
             .OnDelete(DeleteBehavior.Restrict);
            

            builder.HasOne(e => e.UnitInfo)
            .WithMany(e => e.TransactionInfos)
            .HasForeignKey(e => e.UnitInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.StoreInfo)
           .WithMany(e => e.TransactionInfos)
           .HasForeignKey(e => e.StoreInfoId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(e => e.ProductRateInfo)
           .WithMany(e => e.TransactionInfos)
           .HasForeignKey(e => e.ProductRateInfoId)
           .OnDelete(DeleteBehavior.Restrict);
            

        }
    } 
}
