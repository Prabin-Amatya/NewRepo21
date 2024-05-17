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
    public class UnitInfoConfiguration : IEntityTypeConfiguration<UnitInfo>
    {
        public void Configure(EntityTypeBuilder<UnitInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.UnitName).HasMaxLength(200).IsUnicode(true);

            

            builder.HasMany(e => e.ProductRateInfos)
            .WithOne(e => e.UnitInfo)
            .HasForeignKey(e => e.UnitInfoId);

            builder.HasMany(e => e.ProductInfos)
            .WithOne(e => e.UnitInfo)
            .HasForeignKey(e => e.UnitInfoId);

            builder.HasMany(e => e.TransactionInfos)
            .WithOne(e => e.UnitInfo)
            .HasForeignKey(e => e.UnitInfoId);
        }
    }
}
