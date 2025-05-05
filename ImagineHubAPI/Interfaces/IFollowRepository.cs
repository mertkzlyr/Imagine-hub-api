using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IFollowRepository
{
    Task FollowAsync(int followerId, int followeeId);
    Task UnfollowAsync(int followerId, int followeeId);
    Task<(List<User> Users, int TotalCount)> GetFollowersAsync(int userId, int page, int pageSize);
    Task<(List<User> Users, int TotalCount)> GetFollowingAsync(int userId, int page, int pageSize);
}