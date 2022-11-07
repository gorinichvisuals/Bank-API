using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<User>? Users { get; set; }
        public DbSet<Card>? Cards { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=bankapi;Trusted_Connection=True");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.Number)
                .IsUnique();

            modelBuilder.Entity<Card>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Card>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Card>()
                .Property(c => c.Currency)
                .HasConversion<int>();

            modelBuilder.Entity<Card>()
                .Property(c => c.Status)
                .HasConversion<int>();
            
            base.OnModelCreating(modelBuilder); 
        }
    }
}
