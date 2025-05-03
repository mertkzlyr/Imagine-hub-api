using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IFollowRepository
{
    Task FollowAsync(int followerId, int followeeId);
    Task UnfollowAsync(int followerId, int followeeId);
    Task<List<User>> GetFollowersAsync(int userId);
    Task<List<User>> GetFollowingAsync(int userId);
}