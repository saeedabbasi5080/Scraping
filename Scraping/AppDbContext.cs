using Microsoft.EntityFrameworkCore;
using Scraping.Entities;


namespace Scraping
{
    public class AppDbContext : DbContext
    {
        public DbSet<About> Abouts { get; set; }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<Instructions> Instructions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }

    }
}
