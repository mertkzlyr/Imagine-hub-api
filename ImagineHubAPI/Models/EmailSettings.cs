namespace ImagineHubAPI.Models;

public class EmailSettings
{
    public string SmtpUser { get; set; } = null!;
    public string SmtpPass { get; set; } = null!;
    public string SmtpHost { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
}
