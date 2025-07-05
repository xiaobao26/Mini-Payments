namespace Mini_Payments_Backend.Models;

public class Transaction
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    
    // Foreign Key
    public Guid AccountId { get; private set; }
    // Navigation 
    public Account Account { get; private set; }
    
    // Parameterless Constructor for EF Core to materialize this entity 
    private Transaction() { }
    
    // Parametered Constructor for internal use
    // only customer can create Transaction
    // use private to restric  
    private Transaction(Guid id, decimal amount, string currency, TransactionType transactionType, DateTimeOffset createdAt, Guid accountId)
    {
        Id = id;
        Amount = amount;
        Currency = currency;
        TransactionType = transactionType;
        CreatedAt = createdAt;
        AccountId = accountId;
    }
    
    /// <summary>
    ///     Factory method to create new Transaction
    /// </summary>
    /// <param name="amount">Must be greater and equal than 0.00</param>
    /// <param name="currency">e.g.: "USD"</param>
    /// <param name="transactionType">Deposit or Withdraw</param>
    /// <returns>
    ///     A fully initialized <see cref="Transaction"/> with a new <see cref="Transaction.Id"/> and <see cref="Transaction.CreatedAt"/> set to the current UTC time.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="amount"/> is negative, or if <paramref name="currency"/> is null, empty, or whitespace.
    /// </exception>
    public static Transaction Create(decimal amount, string currency, TransactionType transactionType, Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account id is required", nameof(accountId));
        }
        if (amount < 0)
        {
            throw new ArgumentException("Amount must be non-negative", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency is required", nameof(currency));
        }
        
        // Normalize currency code
        currency = currency.Trim().ToUpper();
        
        return new Transaction(
            id: Guid.NewGuid(),
            amount: Math.Round(amount, 2),
            currency: currency,
            transactionType: transactionType,
            createdAt: DateTimeOffset.UtcNow,
            accountId: accountId
        );
    }
}

