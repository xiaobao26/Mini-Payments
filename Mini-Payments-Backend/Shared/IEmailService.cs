namespace Shared;

public interface IEmailService
{
    Task SendEmailAsync(string receiver, string subject, string body, CancellationToken cancellationToken=default);
    string RenderTemplate(string templatePath, object data);
}