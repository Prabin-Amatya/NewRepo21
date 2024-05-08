using Microsoft.EntityFrameworkCore;
using IMS.Infrastructure.Entity_Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IMS.Infrastructure

{
    public class IMSDbContext : DbContext
    {
        public IMSDbContext(DbContextOptions<IMSDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoreConfiguration());
        }
    }
}
