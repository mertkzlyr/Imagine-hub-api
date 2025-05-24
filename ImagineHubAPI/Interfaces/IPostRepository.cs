using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IPostRepository
{
    Task<Result<Post>> AddAsync(Post post);
    Task<Result<Post>> GetByIdAsync(Guid id);
    Task<ResultList<Post>> GetAllPostsAsync(int page, int pageSize, string search);
    Task<ResultList<Post>> GetPostsByUserAsync(int userId, int page, int pageSize);
    Task<Result> LikePostAsync(int userId, Guid postId);
    Task<Result> UnlikePostAsync(int userId, Guid postId);
    Task<Result> UpdatePostAsync(int userId, Guid postId, string description);
    Task<Result> DeletePostAsync(int userId, Guid postId);
}