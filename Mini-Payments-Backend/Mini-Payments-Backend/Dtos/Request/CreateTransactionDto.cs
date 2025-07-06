using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Dtos.Request;

public record CreateTransactionDto(decimal Amount, string Currency, TransactionType TransactionType);

