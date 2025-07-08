using HandlebarsDotNet;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared;

namespace Notification.EmailService;

public class EmailService: IEmailService
{
    private readonly SmtpSettings _smtp;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> smtp, ILogger<EmailService> logger)
    {
        _smtp = smtp.Value;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string receiver, string subject, string body, CancellationToken cancellationToken=default)
    {
        // Email Details
        var smtpServer = _smtp.SmtpServer;
        var smtpPort = _smtp.SmtpPort;
        var senderName = _smtp.SenderName;
        var senderEmail = _smtp.SenderEmail;
        var senderPassword = _smtp.SenderPassword;
        
        // create new email
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
        emailMessage.To.Add(new MailboxAddress(receiver, receiver));
        emailMessage.Subject = subject;
        
        // create email body
        var bodyBuilder =  new BodyBuilder { HtmlBody = body };
        emailMessage.Body = bodyBuilder.ToMessageBody();
        
        // send email
        using (var smtpClient = new SmtpClient())
        {
            try
            {
                // Connect to SMTP Server
                await smtpClient.ConnectAsync(
                        smtpServer, smtpPort,  
                        MailKit.Security.SecureSocketOptions.StartTls,
                        cancellationToken);
                // Authentication
                await smtpClient.AuthenticateAsync(senderEmail, senderPassword, cancellationToken);
                // Send email
                await smtpClient.SendAsync(emailMessage, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
                // try a best-effort cleanup
                if (smtpClient.IsConnected)
                    await smtpClient.DisconnectAsync(true, cancellationToken);
                throw;  // rethrow so MassTransit can retry or dead-letter
            }
        }
    }


    public string RenderTemplate(string templatePath, object data)
    {
        // Load html 
        var template = File.ReadAllText(templatePath);
        
        // compile html by using handlebars
        var compiledTemplate = Handlebars.Compile(template);
        
        // render html with data
        return compiledTemplate(data);
    }
}