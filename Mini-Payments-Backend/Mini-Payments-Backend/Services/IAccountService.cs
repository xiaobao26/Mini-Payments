using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;

namespace Mini_Payments_Backend.Services;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccountAsync(CreateAccountDto dto);
    Task<AccountResponseDto?> GetAccountByIdAsync(Guid id);
}