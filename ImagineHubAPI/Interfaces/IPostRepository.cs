using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IPostRepository
{
    Task<Result<Post>> AddAsync(Post post);
    Task<Result<Post>> GetByIdAsync(Guid id);
}