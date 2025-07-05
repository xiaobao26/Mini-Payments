using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Services;

public class AccountsService: IAccountService
{
    private readonly PaymentsContext _dbcontext;
    private readonly IMapper _mapper;

    public AccountsService(PaymentsContext dbcontext, IMapper mapper)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
    }

    public async Task<AccountResponseDto> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = _mapper.Map<Account>(dto);
        
        _dbcontext.Accounts.Add(account);
        await _dbcontext.SaveChangesAsync();

        var res = _mapper.Map<AccountResponseDto>(account);
        return res;
    }

    public async Task<AccountResponseDto?> GetAccountByIdAsync(Guid id)
    {
        var account = await _dbcontext.Accounts.FindAsync(id);

        var res = _mapper.Map<AccountResponseDto>(account);
        return res;
    }
}