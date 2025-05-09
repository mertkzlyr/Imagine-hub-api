namespace ImagineHubAPI.DTOs.PostDTOs;

public class PostDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
