using Microsoft.EntityFrameworkCore;

namespace Mini_Payments_Backend.Models;

public class PaymentsContext: DbContext
{
    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options) { }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Account mapping
        builder.Entity<Account>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(256);
            e.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(a => a.Balance)
                .HasPrecision(18, 2);
            e.Property(a => a.CreatedAt)
                .IsRequired();
            e.Property(a => a.UpdatedAt)
                .IsRequired();

            e.HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        });
        
        // Transaction mapping
        builder.Entity<Transaction>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            e.Property(t => t.Currency)
                .IsRequired();
            e.Property(t => t.TransactionType)
                .IsRequired();
            e.Property(t => t.CreatedAt)
                .IsRequired();
        });
    }
    
}