namespace ImagineHubAPI.DTOs.CommentDTOs;

public class CommentDto
{

    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string ProfilePicture { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int LikeCount { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}