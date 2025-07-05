using AutoMapper;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Profiles;

public class PaymentsMappingProfile: Profile
{
    public PaymentsMappingProfile()
    {
        // Account
        CreateMap<CreateAccountDto, Account>()
            .ReverseMap();

        CreateMap<AccountResponseDto, Account>()
            .ReverseMap();
        
        // Transaction
        CreateMap<CreateTransactionDto, Transaction>()
            .ReverseMap();

        CreateMap<TransactionResonseDto, Transaction>()
            .ReverseMap();
    }
}