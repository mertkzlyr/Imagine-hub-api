using ImagineHubAPI.Models;

namespace ImagineHubAPI.Interfaces;

public interface IImageService
{
    Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId);
}