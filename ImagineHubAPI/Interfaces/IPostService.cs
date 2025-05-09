using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IPostService
{
    Task<Result<PostDto>> CreatePostAsync(int userId, CreatePostDto createPostDto);
    Task<Result<PostDto>> GetPostByIdAsync(Guid id);
    Task<ResultList<PostDto>> GetAllPostsAsync(int page, int pageSize);
    Task<ResultList<PostDto>> GetPostsByUserAsync(int userId, int page, int pageSize);
    Task<Result> LikePostAsync(int userId, Guid postId);
    Task<Result> UnlikePostAsync(int userId, Guid postId);
    Task<Result> UpdatePostDescriptionAsync(int userId, Guid postId, string description);
}