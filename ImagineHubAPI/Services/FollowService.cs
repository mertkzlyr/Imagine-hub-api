using ImagineHubAPI.DTOs.UserDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Mappers;
using ImagineHubAPI.Models;
using ImagineHubAPI.Repositories;

namespace ImagineHubAPI.Services;

public class FollowService(IFollowRepository followRepository) : IFollowService
{
    public async Task FollowAsync(int followerId, int followeeId)
    {
        await followRepository.FollowAsync(followerId, followeeId);
    }

    public async Task UnfollowAsync(int followerId, int followeeId)
    {
        await followRepository.UnfollowAsync(followerId, followeeId);
    }

    public async Task<List<UserDto>> GetFollowersAsync(int userId)
    {
        var followers = await followRepository.GetFollowersAsync(userId);
        
        var followersDtos = followers.Select(UserMapper.ToDto);

        return followersDtos.ToList();
    }

    public async Task<List<UserDto>> GetFollowingAsync(int userId)
    {
        var followings = await followRepository.GetFollowingAsync(userId);

        var followingDtos = followings.Select(UserMapper.ToDto);
        
        return followingDtos.ToList();
    }
}