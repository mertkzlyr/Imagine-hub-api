namespace ImagineHubAPI.Models;

public class Post
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; }
    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
}