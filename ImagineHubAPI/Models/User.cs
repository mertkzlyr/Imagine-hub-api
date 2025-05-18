namespace ImagineHubAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "user";
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? ProfilePicture { get; set; }
    public int GenerationToken { get; set; }

    public ICollection<UserFollows> Followers { get; set; } = new List<UserFollows>();
    public ICollection<UserFollows> Following { get; set; } = new List<UserFollows>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<PostLike> LikedPosts { get; set; } = new List<PostLike>();
    public ICollection<CommentLike> LikedComments { get; set; } = new List<CommentLike>();
}