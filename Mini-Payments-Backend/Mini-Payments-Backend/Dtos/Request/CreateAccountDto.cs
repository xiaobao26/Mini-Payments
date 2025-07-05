namespace Mini_Payments_Backend.Dtos.Request;

public record CreateAccountDto
(
     string Email,
     string PhoneNumber,
     string UserName
);