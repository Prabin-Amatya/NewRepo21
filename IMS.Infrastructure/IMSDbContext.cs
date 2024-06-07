using Microsoft.EntityFrameworkCore;
using IMS.Infrastructure.Entity_Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Modes.ViewModels;



namespace IMS.Infrastructure

{
    public class IMSDbContext : DbContext
    {
        public IMSDbContext(DbContextOptions<IMSDbContext> options) : base(options) { }

        public DbSet<CustomerReportViewModel> CustomerReportViewModels {  get; set; }
        public DbSet<CustomerDetailedReportViewModel> CustomerDetailedReportViewModels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoreConfiguration());
            builder.ApplyConfiguration(new CategoryInfoConfiguration());
            builder.ApplyConfiguration(new CustomerInfoConfiguration());
            builder.ApplyConfiguration(new UnitInfoConfiguration());
            builder.ApplyConfiguration(new ProductInfoConfiguration());
            builder.ApplyConfiguration(new RackInfoConfiguration());
            builder.ApplyConfiguration(new SupplierInfoConfiguration());
            builder.ApplyConfiguration(new ProductRateInfoConfiguration());
            builder.ApplyConfiguration(new StockInfoConfiguration());
            builder.ApplyConfiguration(new TransactionInfoConfiguration());
            builder.ApplyConfiguration(new ProductInvoiceInfoConfiguration());
            builder.ApplyConfiguration(new ProductInvoiceDetailInfoConfiguration());


        }
    }
}
