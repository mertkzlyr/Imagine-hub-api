using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageService
{
    Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId);
    Task<Result<Models.Image>> GetImageByIdAsync(Guid id, int userId);
    Task<ResultList<Models.Image>> GetImagesByUserIdAsync(int userId, int page, int pageSize);
    Task<User?> GetUserByIdAsync(int userId);
}