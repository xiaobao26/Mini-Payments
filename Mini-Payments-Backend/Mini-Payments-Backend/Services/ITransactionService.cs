using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;

namespace Mini_Payments_Backend.Services;

public interface ITransactionService
{
    Task<TransactionResonseDto> CreateTransactionAsync(Guid accountId, CreateTransactionDto dto);
    Task<IEnumerable<TransactionResonseDto>> GetAllTransactionsByAccountId(Guid accountId);
}