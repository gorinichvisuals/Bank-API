using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bank_API.DataAccessLayer.DataContext
{
    public class AppDataContext : DbContext
    {
        public AppDataContext()
        {

        }

        public AppDataContext(DbContextOptions<AppDataContext> dbContextOptions) 
            : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=localhost\\SQLEXPRESS;Database=bank-api;Trusted_Connection=True")
                    .UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Phone)
                .IsUnique();
        }
    }
}
