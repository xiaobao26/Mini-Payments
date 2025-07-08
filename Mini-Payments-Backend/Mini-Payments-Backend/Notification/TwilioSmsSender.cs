using Microsoft.Extensions.Options;
using Mini_Payments_Backend.Notification;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class TwilioSmsSender : ISmsSender
{
    private readonly TwilioSettings _settings;
    private readonly ILogger<TwilioSmsSender> _logger;

    public TwilioSmsSender(
        IOptions<TwilioSettings> options,
        ILogger<TwilioSmsSender> logger)
    {
        _settings = options.Value;
        _logger   = logger;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
    }

    public async Task SendSmsAsync(string to, string message)
    {
        try
        {
            var msg = await MessageResource.CreateAsync(
                to:   new PhoneNumber(to),
                from: new PhoneNumber(_settings.FromNumber),
                body: message
            );

            _logger.LogInformation(
                "Twilio SMS sent. SID={Sid}, Status={Status}, To={To}, From={From}",
                msg.Sid, msg.Status, to, _settings.FromNumber);

            if (msg.ErrorCode != null)
            {
                _logger.LogWarning(
                    "Twilio returned an error: Code={ErrorCode}, Message={ErrorMessage}",
                    msg.ErrorCode, msg.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS via Twilio to {To}", to);
            throw;    // or swallow if you really don’t want to bubble up
        }
    }
}