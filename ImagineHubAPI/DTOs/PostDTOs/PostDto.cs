namespace ImagineHubAPI.DTOs.PostDTOs;

public class PostDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
