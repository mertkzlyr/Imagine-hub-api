using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageService
{
    Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId);
    Task<Result<Image>> GetImageByIdAsync(Guid id, int userId);
    Task<ResultList<Image>> GetImagesByUserIdAsync(int userId, int page, int pageSize);
}