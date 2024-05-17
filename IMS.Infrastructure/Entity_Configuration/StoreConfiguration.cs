using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IMS.Modes.Entity;


namespace IMS.Infrastructure.Entity_Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<StoreInfo>
    {
        public void Configure(EntityTypeBuilder<StoreInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.StoreName);
            builder.Property(e => e.StoreName).HasMaxLength(200).IsUnicode(true);
            builder.Property(e => e.Address).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.PhoneNumber).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.RegistrationNo).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.PanNo).HasMaxLength(50).IsUnicode(true);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).IsUnicode(true).HasDefaultValueSql("GETDATE()").HasColumnType("DATETIME");
            builder.Property(e => e.CreatedBy).IsUnicode(true);
            builder.Property(e => e.ModifiedDate).HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ModifiedBy).IsUnicode(true);

            builder.HasMany(e => e.CategoryInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.CustomerInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductRateInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductInvoiceInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.RackInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.StockInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.SupplierInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.TransactionInfos)
            .WithOne(e => e.StoreInfo)
            .HasForeignKey(e => e.StoreInfoId);
        }
    }

}
