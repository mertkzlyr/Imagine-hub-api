using ImagineHubAPI.DTOs.CommentDTOs;
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

    public async Task<Result<PostByIdDto>> GetPostByIdAsync(Guid id)
    {
        var postResult = await postRepository.GetByIdAsync(id);

        if (!postResult.Success)
        {
            return new Result<PostByIdDto>
            {
                Success = false,
                Message = postResult.Message,
                Data = null
            };
        }

        var postDto = new PostByIdDto
        {
            Id = postResult.Data.Id,
            UserId = postResult.Data.UserId,
            Username = postResult.Data.User.Username,
            Name = postResult.Data.User.Name,
            Surname = postResult.Data.User.Surname,
            Description = postResult.Data.Description,
            ImageUrl = postResult.Data.ImageUrl,
            CreatedAt = postResult.Data.CreatedAt,
            LikeCount = postResult.Data.Likes.Count,
            CommentCount = postResult.Data.Comments.Count,
            Comments = postResult.Data.Comments
                .Where(c => c.ParentId == null)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    Comment = c.Comment,
                    ParentId = c.ParentId,
                    LikeCount = c.Likes.Count,
                    CreatedAt = c.CreatedAt,
                    Replies = postResult.Data.Comments
                        .Where(r => r.ParentId == c.Id)
                        .Select(r => new CommentDto
                        {
                            Id = r.Id,
                            UserId = r.UserId,
                            Username = r.User.Username,
                            Comment = r.Comment,
                            ParentId = r.ParentId,
                            LikeCount = r.Likes.Count,
                            CreatedAt = r.CreatedAt,
                            Replies = []
                        }).ToList()
                }).ToList()
        };

        return new Result<PostByIdDto>
        {
            Success = true,
            Message = postResult.Message,
            Data = postDto
        };
    }

    public async Task<ResultList<PostDto>> GetAllPostsAsync(int page, int pageSize)
    {
        var result = await postRepository.GetAllPostsAsync(page, pageSize);

        var postDtos = result.Data.Select(p => new PostDto
        {
            Id = p.Id,
            UserId = p.UserId,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            Username = p.User.Username,
            Name = p.User.Name,
            Surname = p.User.Surname,
            LikeCount = p.Likes.Count,
            CommentCount = p.Comments.Count
        }).ToList();

        return new ResultList<PostDto>
        {
            Success = true,
            Message = result.Message,
            Data = postDtos,
            Pagination = result.Pagination
        };
    }

    public async Task<ResultList<PostDto>> GetPostsByUserAsync(int userId, int page, int pageSize)
    {
        var result = await postRepository.GetPostsByUserAsync(userId, page, pageSize);

        var postDtos = result.Data.Select(p => new PostDto
        {
            Id = p.Id,
            UserId = p.UserId,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            Username = p.User.Username,
            Name = p.User.Name,
            Surname = p.User.Surname,
            LikeCount = p.Likes.Count,
            CommentCount = p.Comments.Count
        }).ToList();

        return new ResultList<PostDto>
        {
            Success = true,
            Message = result.Message,
            Data = postDtos,
            Pagination = result.Pagination
        };
    }

    public async Task<Result> LikePostAsync(int userId, Guid postId)
    {
        return await postRepository.LikePostAsync(userId, postId);
    }

    public async Task<Result> UnlikePostAsync(int userId, Guid postId)
    {
        return await postRepository.UnlikePostAsync(userId, postId);
    }

    public async Task<Result> UpdatePostDescriptionAsync(int userId, Guid postId, string description)
    {
        return await postRepository.UpdatePostAsync(userId, postId, description);
    }
}