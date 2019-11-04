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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=OrderDB.db");

            base.OnConfiguring(optionsBuilder);
        }

    }
}
