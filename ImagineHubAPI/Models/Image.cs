namespace ImagineHubAPI.Models;

public class Image
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Prompt { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}