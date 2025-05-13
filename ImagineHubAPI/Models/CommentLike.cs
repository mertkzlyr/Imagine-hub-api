namespace ImagineHubAPI.Models;

public class CommentLike
{
    public int UserId { get; set; }
    public User User { get; set; }

    public Guid CommentId { get; set; }
    public PostComment Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}