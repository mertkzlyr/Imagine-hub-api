namespace ImagineHubAPI.Models;

public class PostComment
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public Guid? ParentId { get; set; }
    public PostComment ParentComment { get; set; }
    public ICollection<PostComment> Replies { get; set; } = new List<PostComment>();
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
}