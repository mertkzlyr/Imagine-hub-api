namespace ImagineHubAPI.Models;

public class UserFollows
{
    public int FollowerId { get; set; }
    public User Follower { get; set; }

    public int FolloweeId { get; set; }
    public User Followee { get; set; }

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}