using Microsoft.EntityFrameworkCore;
using ScrapingApplication.Entities;


namespace ScrapingApplication.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<Attachment> Abouts { get; set; }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<Instructions> Instructions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }

    }
}
