using MassTransit;
using Shared;

namespace Notification.Consumers;

public class SendTransactionDepositEmailConsumer: IConsumer<Events.MoneyDepositedEvent>
{
    private readonly ILogger<SendTransactionDepositEmailConsumer> _logger;
    private readonly IEmailService _emailService;

    public SendTransactionDepositEmailConsumer(ILogger<SendTransactionDepositEmailConsumer> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<Events.MoneyDepositedEvent> context)
    {
        
    }
}