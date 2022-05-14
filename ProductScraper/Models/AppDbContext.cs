﻿using Microsoft.EntityFrameworkCore;

namespace ProductScraper.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductChange> ProductChanges { get; set; }

    }
}
