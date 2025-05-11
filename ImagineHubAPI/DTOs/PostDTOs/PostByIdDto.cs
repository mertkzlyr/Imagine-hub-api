using ImagineHubAPI.DTOs.CommentDTOs;

namespace ImagineHubAPI.DTOs.PostDTOs;

public class PostByIdDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    public DateTime CreatedAt { get; set; }
}