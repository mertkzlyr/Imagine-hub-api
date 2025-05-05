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

    public async Task<ResultList<UserDto>> GetFollowersAsync(int userId, int page, int pageSize)
    {
        var (followers, totalCount) = await followRepository.GetFollowersAsync(userId, page, pageSize);

        var followerDtos = followers.Select(UserMapper.ToDto).ToList();

        var pagination = new PaginationInformation
        {
            From = (page - 1) * pageSize + 1,
            To = Math.Min(page * pageSize, totalCount),
            Total = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return new ResultList<UserDto>
        {
            Success = true,
            Message = "Followers retrieved successfully.",
            Data = followerDtos,
            Pagination = pagination
        };
    }

    public async Task<ResultList<UserDto>> GetFollowingAsync(int userId, int page, int pageSize)
    {
        var (following, totalCount) = await followRepository.GetFollowingAsync(userId, page, pageSize);

        var followingDtos = following.Select(UserMapper.ToDto).ToList();

        var pagination = new PaginationInformation
        {
            From = (page - 1) * pageSize + 1,
            To = Math.Min(page * pageSize, totalCount),
            Total = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return new ResultList<UserDto>
        {
            Success = true,
            Message = "Following retrieved successfully.",
            Data = followingDtos,
            Pagination = pagination
        };
    }
}