using System.Net;
using System.Net.Mail;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendMagicLinkAsync(string toEmail, string magicLinkUrl)
    {
        var message = new MailMessage();
        message.To.Add(toEmail);
        message.Subject = "Your Magic Password Reset Link";
        message.Body = $"Click this link to reset your password:\n{magicLinkUrl}\n\nThis link will expire in 30 minutes.";
        message.IsBodyHtml = false;
        message.From = new MailAddress(_emailSettings.SmtpUser);

        using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(message);
    }
}