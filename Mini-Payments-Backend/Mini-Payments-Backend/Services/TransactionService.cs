using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Services;

public class TransactionService: ITransactionService
{
    private readonly PaymentsContext _paymentsContext;
    private readonly IMapper _mapper;
    public TransactionService(PaymentsContext paymentsContext, IMapper mapper)
    {
        _paymentsContext = paymentsContext;
        _mapper = mapper;
    }
    
    public async Task<TransactionResonseDto> CreateTransactionAsync(Guid accountId, CreateTransactionDto dto)
    {
        var account = await _paymentsContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account is null)
        {
            throw new KeyNotFoundException($"Account: [{accountId}] cannot be found");
        }
        
        // build the domain transaction (validation + normalization)
        var transaction = Transaction.Create(
            accountId: accountId,
            amount: dto.Amount,
            currency: dto.Currency,
            transactionType: dto.TransactionType
        );
        
        // apply to the account (will throw if withdraw & insufficient funds)
        account.ApplyTransaction(transaction);
        
        // persist both sides
        _paymentsContext.Transactions.Add(transaction);
        await _paymentsContext.SaveChangesAsync();

        var res = _mapper.Map<TransactionResonseDto>(transaction);
        return res;
    }


    public async Task<List<TransactionResonseDto>> GetAllTransactionsByAccountId(Guid accountId)
    {
        var list = await _paymentsContext.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return list.Select(t => _mapper.Map<TransactionResonseDto>(t)).ToList();
    }
}