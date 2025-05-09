using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Repositories;

public class PostRepository(DataContext context) : IPostRepository
{
    public async Task<Result<Post>> AddAsync(Post post)
    {
        try
        {
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync(); // Persist changes to the database
            return new Result<Post>
            {
                Success = true,
                Message = "Post created successfully.",
                Data = post
            };
        }
        catch (Exception ex)
        {
            return new Result<Post>
            {
                Success = false,
                Message = $"An error occurred while creating the post: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Result<Post>> GetByIdAsync(Guid id)
    {
        try
        {
            var post = await context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return new Result<Post>
                {
                    Success = false,
                    Message = "Post not found.",
                    Data = null
                };
            }

            return new Result<Post>
            {
                Success = true,
                Message = "Post retrieved successfully.",
                Data = post
            };
        }
        catch (Exception ex)
        {
            return new Result<Post>
            {
                Success = false,
                Message = $"An error occurred while retrieving the post: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Result> LikePostAsync(int userId, Guid postId)
    {
        var existingLike = await context.PostLikes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        
        if (existingLike != null)
        {
            return new Result
            {
                Success = false,
                Message = "Post already liked."
            };
        }
        
        var like = new PostLike
        {
            UserId = userId,
            PostId = postId
        };
        
        context.PostLikes.Add(like);
        await context.SaveChangesAsync();

        return new Result
        {
            Success = true,
            Message = "Post liked successfully."
        };
    }

    public async Task<Result> UnlikePostAsync(int userId, Guid postId)
    {
        var like = await context.PostLikes
            .FirstOrDefaultAsync(pl => pl.UserId == userId && pl.PostId == postId);

        if (like == null)
        {
            return new Result
            {
                Success = false,
                Message = "Like not found."
            };
        }

        context.PostLikes.Remove(like);
        await context.SaveChangesAsync();

        return new Result
        {
            Success = true,
            Message = "Post unliked successfully."
        };
    }
}