using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class FollowRepository(DataContext context) : IFollowRepository
{
    public async Task FollowAsync(int followerId, int followeeId)
    {
        if (followerId == followeeId)
        {
            throw new InvalidOperationException("Cannot follow yourself.");
        }

        var alreadyFollowing = await context.UserFollows
            .AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        
        if (!alreadyFollowing)
        {
            var follow = new UserFollows
            {
                FollowerId = followerId,
                FolloweeId = followeeId,
            };
            context.UserFollows.Add(follow);
            await context.SaveChangesAsync();
        }
    }

    public async Task UnfollowAsync(int followerId, int followeeId)
    {
        var follow = await context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

        if (follow != null)
        {
            context.UserFollows.Remove(follow);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<User>> GetFollowersAsync(int userId)
    {
        return await context.UserFollows
            .Where(f => f.FolloweeId == userId)
            .Select(f => f.Follower)
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(int userId)
    {
        return await context.UserFollows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Followee)
            .ToListAsync();
    }
}