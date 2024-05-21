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
    public class SupplierInfoConfiguration : IEntityTypeConfiguration<SupplierInfo>
    {
        public void Configure(EntityTypeBuilder<SupplierInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.SupplierName).HasMaxLength(200).IsUnicode(true).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.PhoneNumber).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.ContactPerson).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(50).IsUnicode(true).IsRequired();
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("datetime");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
            .WithMany(e => e.SupplierInfos)
            .HasForeignKey(e => e.StoreInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ProductRateInfos)
            .WithOne(e => e.SupplierInfo)
            .HasForeignKey(e => e.SupplierInfoId);

        }
    }
}
