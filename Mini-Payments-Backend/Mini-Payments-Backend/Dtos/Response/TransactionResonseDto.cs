using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Dtos.Response;

public record TransactionResonseDto(
    Guid Id,
    decimal Amount,
    string Currency,
    TransactionType TransactionType,
    DateTimeOffset CreateAt
);
