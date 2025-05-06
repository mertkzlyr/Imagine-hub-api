using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IPostService
{
    Task<Result<PostDto>> CreatePostAsync(int userId, CreatePostDto createPostDto);
    Task<Result<PostDto>> GetPostByIdAsync(Guid id);
}