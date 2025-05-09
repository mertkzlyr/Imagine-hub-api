namespace ImagineHubAPI.Models;

public class PostLike
{
    public int UserId { get; set; }
    public User User { get; set; }

    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}