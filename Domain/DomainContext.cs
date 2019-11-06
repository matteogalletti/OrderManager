using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductOption> ProductOptions { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<CustomOption> CustomOptions { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.CustomOptions)
                .WithOne(co => co.Order)
                .HasForeignKey(co => co.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<CustomOption>()
                .HasKey(o => new { o.OrderId, o.ProductId, o.OptionId });
            modelBuilder.Entity<CustomOption>()
                .HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductId);
            modelBuilder.Entity<CustomOption>()
                .HasOne(o => o.ProductOption).WithMany().HasForeignKey(o => o.OptionId);
        }

    }
}
