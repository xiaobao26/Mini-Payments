using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;
using Mini_Payments_Backend.Notification;
using Shared;

namespace Mini_Payments_Backend.Services;

public class TransactionService: ITransactionService
{
    private readonly PaymentsContext _paymentsContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publish;
    private readonly ISmsSender _smsSender;
    public TransactionService(PaymentsContext paymentsContext, IMapper mapper, IPublishEndpoint publish, ISmsSender smsSender)
    {
        _paymentsContext = paymentsContext;
        _mapper = mapper;
        _publish = publish;
        _smsSender = smsSender;
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
        
        // Broadcast event
        if (transaction.TransactionType == TransactionType.Deposit)
        {
            await _publish.Publish(new Events.MoneyDepositedEvent(account.Id, account.Email, account.UserName,transaction.Amount, account.Balance, transaction.CreatedAt));
        }
        else if (transaction.TransactionType == TransactionType.Withdraw)
        {
            await _publish.Publish(new Events.MoneyWithdrawEvent(account.Id, account.Email, account.UserName, transaction.Amount, account.Balance, transaction.CreatedAt));
        }
        
        // Send SMS 
        var smsBody = transaction.TransactionType == TransactionType.Deposit
            ? $"Deposit of {transaction.Amount:C} received. New balance: {account.Balance:C}."
            : $"Withdrawal of {transaction.Amount:C} processed. New balance: {account.Balance:C}.";
        
        try
        {
            // account.PhoneNumber must be E.164 formatted (e.g. "+15551234567")
            await _smsSender.SendSmsAsync(account.PhoneNumber, smsBody);
        }
        catch (Exception ex)
        {
            // Log and swallow—don’t break the happy path
            // (inject ILogger<TransactionService> if you want detailed logs)
            Console.Error.WriteLine($"Failed to send SMS: {ex.Message}");
        }
        

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