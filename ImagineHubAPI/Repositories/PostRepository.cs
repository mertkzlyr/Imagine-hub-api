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
                .Include(p => p.User) // Include User to get details of the user who posted
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
}