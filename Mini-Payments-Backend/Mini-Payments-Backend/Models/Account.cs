using System.ComponentModel.DataAnnotations;

namespace Mini_Payments_Backend.Models;

public class Account
{
    public Guid Id { get; private set; }
    
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; private set; }
    
    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; private set; }
    
    [Required, StringLength(20)]
    public string UserName { get; private set; }
    public decimal Balance { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    
    // Nevigation: One Account -> Many Transaction  
    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    
    // Parameterless Constructor for EF Core to materialize this entity
    private Account() { }

    // Parametered Constructor for internal use
    public Account(string userName, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("UserName is required", nameof(userName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        }

        Id = Guid.NewGuid();
        Email = email.Trim().ToLowerInvariant();
        PhoneNumber = phoneNumber.Trim();
        UserName = userName.Trim();
        Balance = 0m;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    
    // Domain Behaviour
    public void ApplyTransaction(Transaction transaction)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }
        if (transaction.AccountId != Id)
        {
            throw new InvalidOperationException("Wrong account!");
        }

        var operation = transaction.TransactionType;
        if (operation == TransactionType.Withdraw)
        {
            if (Balance < transaction.Amount)
            {
                throw new InvalidOperationException("Not enough money");
            }
            Balance -= transaction.Amount;
        }
        else
        {
            Balance += transaction.Amount;
        }
        UpdatedAt = DateTimeOffset.UtcNow;
        _transactions.Add(transaction);
    }
}