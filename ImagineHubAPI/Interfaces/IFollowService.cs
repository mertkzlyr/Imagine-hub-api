using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IFollowService
{
    Task FollowAsync(int followerId, int followeeId);
    Task UnfollowAsync(int followerId, int followeeId);
    Task<ResultList<UserDto>> GetFollowersAsync(int userId, int page, int pageSize);
    Task<ResultList<UserDto>> GetFollowingAsync(int userId, int page, int pageSize);
}