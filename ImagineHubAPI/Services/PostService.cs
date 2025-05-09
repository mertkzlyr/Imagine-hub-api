using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class PostService(IPostRepository postRepository) : IPostService
{
    public async Task<Result<PostDto>> CreatePostAsync(int userId, CreatePostDto createPostDto)
    {
        // Validate incoming data
        if (string.IsNullOrWhiteSpace(createPostDto.Description))
        {
            return new Result<PostDto>
            {
                Success = false,
                Message = "Description is required.",
                Data = null
            };
        }

        // Create the post entity
        var post = new Post
        {
            UserId = userId,
            Description = createPostDto.Description,
            ImageUrl = createPostDto.ImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        // Call the repository to add the post
        var postResult = await postRepository.AddAsync(post);

        // If repository failed to add post, return the failure result
        if (!postResult.Success)
        {
            return new Result<PostDto>
            {
                Success = false,
                Message = postResult.Message,
                Data = null
            };
        }

        // Map to DTO for the response
        var postDto = new PostDto
        {
            Id = postResult.Data.Id,
            UserId = postResult.Data.UserId,
            Description = postResult.Data.Description,
            ImageUrl = postResult.Data.ImageUrl,
            CreatedAt = postResult.Data.CreatedAt
        };

        return new Result<PostDto>
        {
            Success = true,
            Message = postResult.Message,
            Data = postDto
        };
    }

    public async Task<Result<PostDto>> GetPostByIdAsync(Guid id)
    {
        var postResult = await postRepository.GetByIdAsync(id);

        if (!postResult.Success)
        {
            return new Result<PostDto>
            {
                Success = false,
                Message = postResult.Message,
                Data = null
            };
        }

        var postDto = new PostDto
        {
            Id = postResult.Data.Id,
            UserId = postResult.Data.UserId,
            Username = postResult.Data.User.Username,
            Name = postResult.Data.User.Name,
            Surname = postResult.Data.User.Surname,
            Description = postResult.Data.Description,
            ImageUrl = postResult.Data.ImageUrl,
            CreatedAt = postResult.Data.CreatedAt
        };

        return new Result<PostDto>
        {
            Success = true,
            Message = postResult.Message,
            Data = postDto
        };
    }
}