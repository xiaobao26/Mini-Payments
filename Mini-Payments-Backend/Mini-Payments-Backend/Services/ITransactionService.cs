using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;

namespace Mini_Payments_Backend.Services;

public interface ITransactionService
{
    Task<TransactionResonseDto> CreateTransactionAsync(Guid accountId, CreateTransactionDto dto);
    Task<List<TransactionResonseDto>> GetAllTransactionsByAccountId(Guid accountId);
}