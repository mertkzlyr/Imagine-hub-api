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

    public async Task<ResultList<Post>> GetAllPostsAsync(int page, int pageSize)
    {
        var totalPosts = await context.Posts.CountAsync();
        var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

        var posts = await context.Posts
            .Include(p => p.User)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ResultList<Post>
        {
            Success = true,
            Message = "Posts retrieved successfully.",
            Data = posts,
            Pagination = new PaginationInformation
            {
                From = ((page - 1) * pageSize) + 1,
                To = ((page - 1) * pageSize) + posts.Count,
                Total = totalPosts,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            }
        };
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