using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IFollowService
{
    Task FollowAsync(int followerId, int followeeId);
    Task UnfollowAsync(int followerId, int followeeId);
    Task<List<UserDto>> GetFollowersAsync(int userId);
    Task<List<UserDto>> GetFollowingAsync(int userId);
}