using GenerateWordFile.Models;
using Microsoft.EntityFrameworkCore;

namespace GenerateWordFile.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.FinCode)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.CardNumber)
                .IsUnique();
        }

    }
}