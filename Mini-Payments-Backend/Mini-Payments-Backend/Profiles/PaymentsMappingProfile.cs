using AutoMapper;
using Mini_Payments_Backend.Dtos.Request;
using Mini_Payments_Backend.Dtos.Response;
using Mini_Payments_Backend.Models;

namespace Mini_Payments_Backend.Profiles;

public class PaymentsMappingProfile: Profile
{
    public PaymentsMappingProfile()
    {
        CreateMap<CreateAccountDto, Account>()
            .ConstructUsing(dto => new Account(dto.UserName, dto.Email, dto.PhoneNumber))
            .ReverseMap();

        CreateMap<AccountResponseDto, Account>()
            .ReverseMap();
    }
}