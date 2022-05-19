using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductScraper.Domain;

namespace ProductScraper.DataAccess
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductChange> ProductChanges { get; set; }

        public DbSet<TrackingUser> TrackingUsers { get; set; }

    }
}
