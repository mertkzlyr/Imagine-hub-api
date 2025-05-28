using ImagineHubAPI.DTOs.CommentDTOs;
using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Helpers;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class PostService(IPostRepository postRepository, IRedisService redisService) : IPostService
{
    public async Task<Result<PostDto>> CreatePostAsync(int userId, CreatePostDto createPostDto)
    {
        if (string.IsNullOrWhiteSpace(createPostDto.Description))
        {
            return new Result<PostDto>
            {
                Success = false,
                Message = "Description is required."
            };
        }

        var postId = Guid.NewGuid();
        string? imageFileName = null;

        if (createPostDto.Picture != null)
        {
            imageFileName = await PictureSaver.SavePostPicture(createPostDto.Picture, postId);
        }

        var post = new Post
        {
            Id = postId,
            UserId = userId,
            Description = createPostDto.Description,
            ImageUrl = imageFileName,
            CreatedAt = DateTime.UtcNow
        };

        var postResult = await postRepository.AddAsync(post);
        await redisService.RemoveByPatternAsync("posts:*");

        if (!postResult.Success)
        {
            return new Result<PostDto>
            {
                Success = false,
                Message = postResult.Message
            };
        }

        var postDto = new PostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Description = post.Description,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt
        };

        return new Result<PostDto>
        {
            Success = true,
            Message = "Post created successfully.",
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
            ProfilePicture = postResult.Data.User.ProfilePicture,
            Description = postResult.Data.Description,
            ImageUrl = postResult.Data.ImageUrl,
            CreatedAt = postResult.Data.CreatedAt,
            LikeCount = postResult.Data.Likes.Count,
            CommentCount = postResult.Data.Comments.Count,
            Comments = BuildCommentTree(postResult.Data.Comments.ToList())
        };

        return new Result<PostByIdDto>
        {
            Success = true,
            Message = postResult.Message,
            Data = postDto
        };
    }

    public async Task<ResultList<PostDto>> GetAllPostsAsync(int page, int pageSize, string search)
    {
        string cacheKey = $"posts:page={page}:size={pageSize}:search={search}";
        var cached = await redisService.GetAsync<ResultList<PostDto>>(cacheKey);
        if (cached != null)
            return cached;

        var result = await postRepository.GetAllPostsAsync(page, pageSize, search);

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
            ProfilePicture = p.User.ProfilePicture,
            LikeCount = p.Likes.Count,
            CommentCount = p.Comments.Count
        }).ToList();

        var finalResult = new ResultList<PostDto>
        {
            Success = true,
            Message = result.Message,
            Data = postDtos,
            Pagination = result.Pagination
        };

        // Cache result for 5 minutes
        await redisService.SetAsync(cacheKey, finalResult, TimeSpan.FromMinutes(5));

        return finalResult;
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

    public async Task<Result> DeletePostAsync(int userId, Guid postId)
    {
        var result = await postRepository.DeletePostAsync(userId, postId);

        if (result.Success)
        {
            await redisService.RemoveByPatternAsync("posts:*");
        }

        return result;
    }

    private List<CommentDto> BuildCommentTree(List<PostComment> allComments, Guid? parentId = null)
    {
        return allComments
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Username = c.User.Username,
                ProfilePicture = c.User.ProfilePicture,
                Comment = c.Comment,
                ParentId = c.ParentId,
                LikeCount = c.Likes.Count,
                CreatedAt = c.CreatedAt,
                Replies = BuildCommentTree(allComments, c.Id)  // Recursive call
            })
            .ToList();
    }

}