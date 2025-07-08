namespace Shared;

public static class Events
{
    public record AccountCreatedEvent(Guid AccountId, string Email, string UserName, DateTimeOffset CreatedAt);
    
    public record MoneyDepositedEvent(Guid AccountId, string Email, string UserName, decimal Amount, decimal NewBalance, DateTimeOffset CreatedAt);
    
    public record MoneyWithdrawEvent(Guid AccountId, string Email, string UserName, decimal Amount, decimal NewBalance, DateTimeOffset CreatedAt);

}