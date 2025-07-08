using MassTransit;
using Shared;

namespace Notification.Consumers;

public class SendAccountCreatedEmailConsumer: IConsumer<Events.AccountCreatedEvent>
{
    private readonly ILogger<SendAccountCreatedEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public SendAccountCreatedEmailConsumer(ILogger<SendAccountCreatedEmailConsumer> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<Events.AccountCreatedEvent> context)
    {
        _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [Start-Sending] {DateTime.UtcNow}");
        
        var message = context.Message;
        var emailData = new
        {
            userName = message.UserName,
            accountId = message.AccountId,
            createdAt = message.CreatedAt
            
        };
        string emailBody = _emailService.RenderTemplate("EmailTemplates/AccountCreated.hbs", emailData);
        await _emailService.SendEmailAsync(message.Email, "Create Account Successful", emailBody);
        
        _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [End-Sending] {DateTime.UtcNow}");
    }
}