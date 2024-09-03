namespace Visa.Checkout.DAL.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using Visa.Checkout.Entity;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PricingRule> PricingRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricingRule>().ToTable("PricingRule");
        }
    }
}
