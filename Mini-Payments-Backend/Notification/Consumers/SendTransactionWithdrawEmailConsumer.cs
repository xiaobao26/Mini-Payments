using MassTransit;
using Shared;

namespace Notification.Consumers;

public class SendTransactionWithdrawEmailConsumer: IConsumer<Events.MoneyWithdrawEvent>
{
    private readonly ILogger<SendTransactionDepositEmailConsumer> _logger;
    private readonly IEmailService _emailService;
    
    public SendTransactionWithdrawEmailConsumer(ILogger<SendTransactionDepositEmailConsumer> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<Events.MoneyWithdrawEvent> context)
    {
        _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [Start-Sending] {DateTime.UtcNow}");
        // start
        // var message = context.Message;
        //
        // // use dbContext and accountId find account
        // var emailData = new
        // {
        //     userName = account.UserName;
        //     useremail = account.Email;
        // };
        // string emailBody = _emailService.RenderTemplate("Templates/ResetPasswordEmailTemplate.hbs", emailData);
        // await _emailService.SendEmailAsync(account.Email, "Transaction", emailBody);
        
        // end
        _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [End-Sending] {DateTime.UtcNow}");
    } 
}