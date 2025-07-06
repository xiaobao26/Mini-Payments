using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Dtos.Response;

public class TransactionResonseDto()
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTimeOffset CreateAt { get; set; }
};



