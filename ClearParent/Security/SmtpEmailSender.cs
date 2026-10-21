using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
namespace ClearParent.Security;

// Credentials come from configuration providers (user secrets/environment), never source control.
public sealed class SmtpEmailSender(IConfiguration config) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var host = config["Email:Host"] ?? throw new InvalidOperationException("Configure the email transport before registering accounts.");
        using var client = new SmtpClient(host, config.GetValue("Email:Port", 587)) { EnableSsl = true };
        if (config["Email:Username"] is string username)
            client.Credentials = new NetworkCredential(username, config["Email:Password"]);
        using var message = new MailMessage(config["Email:From"] ?? throw new InvalidOperationException("Email sender is required."), email, subject, htmlMessage) { IsBodyHtml = true };
        await client.SendMailAsync(message);
    }
}
