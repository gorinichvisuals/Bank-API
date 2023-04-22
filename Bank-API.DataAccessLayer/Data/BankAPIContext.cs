namespace Bank_API.DataAccessLayer.DataContext;

public class BankAPIContext : DbContext
{
    public BankAPIContext()
    {

    }

    public BankAPIContext(DbContextOptions<BankAPIContext> dbContextOptions) 
        : base(dbContextOptions)
    {
        
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Card>? Cards { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=bankapi;Trusted_Connection=True;TrustServerCertificate=True");
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
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<User>()
            .Property(u => u.UpdatedAt)
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
            .HasConversion<string>();

        modelBuilder.Entity<Card>()
            .Property(c => c.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.UpdatedAt)
            .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Cards)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<Card>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Card)
            .HasForeignKey(t => t.CardId);

        base.OnModelCreating(modelBuilder); 
    }
}