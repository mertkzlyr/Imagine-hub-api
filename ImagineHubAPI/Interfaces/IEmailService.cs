namespace ImagineHubAPI.Interfaces;

public interface IEmailService
{
    Task SendMagicLinkAsync(string toEmail, string magicLink);
}