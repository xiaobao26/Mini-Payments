using AutoMapper;
using MassTransit;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;
using Shared;

namespace Mini_Payments_Backend.Services;

public class AccountsService: IAccountService
{
    private readonly PaymentsContext _dbcontext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;

    public AccountsService(PaymentsContext dbcontext, IMapper mapper, IPublishEndpoint publisher)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<AccountResponseDto> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = _mapper.Map<Account>(dto);
        
        // 1.Update database
        _dbcontext.Accounts.Add(account);
        await _dbcontext.SaveChangesAsync();
        
        // 2.Broadcast event
        await _publisher.Publish(
            new Events.AccountCreatedEvent(account.Id, account.Email, account.UserName, account.CreatedAt));
        
        
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