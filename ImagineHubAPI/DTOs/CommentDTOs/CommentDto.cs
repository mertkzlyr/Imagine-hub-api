namespace ImagineHubAPI.DTOs.CommentDTOs;

public class CommentDto
{

    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}