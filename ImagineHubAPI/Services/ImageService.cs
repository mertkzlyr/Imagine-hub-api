using System.Text;
using System.Text.Json;
using ImagineHubAPI.DTOs.ImageDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class ImageService(IImageRepository imageRepository, HttpClient httpClient) : IImageService
{
    public async Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId)
    {
        var payload = new { prompt = prompt, size = "512x512" };
        var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("http://localhost:8000/openai/generateimage", json);

        if (!response.IsSuccessStatusCode)
            return new Result<string> { Success = false, Message = "Failed to generate image" };

        var resultStream = await response.Content.ReadAsStreamAsync();

        var result = await JsonSerializer.DeserializeAsync<OpenAIImageResponse>(resultStream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result == null || result.Data == null || result.Data.Count == 0)
            return new Result<string> { Success = false, Message = "Invalid response from image service" };

        var image = new Image
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Prompt = prompt,
            ImageUrl = result.Data[0].Url,
            CreatedAt = DateTime.UtcNow
        };

        await imageRepository.SaveImageAsync(image);

        return new Result<string> { Success = true, Data = image.ImageUrl };
        
    }

    public async Task<Result<Image>> GetImageByIdAsync(Guid id, int userId)
    {
        var image = await imageRepository.GetByIdAsync(id, userId);

        if (image == null)
            return new Result<Image> { Success = false, Message = "Image not found" };

        return new Result<Image> { Success = true, Data = image };
    }

    public async Task<ResultList<Image>> GetImagesByUserIdAsync(int userId, int page, int pageSize)
    {
        // Fetch paged image list
        var images = await imageRepository.GetByUserIdAsync(userId, page, pageSize);

        var totalPages = (int)Math.Ceiling(images.Count / (double)pageSize);
        var from = ((page - 1) * pageSize) + 1;
        var to = from + images.Count - 1;

        var pagination = new PaginationInformation
        {
            From = from,
            To = to,
            Total = images.Count ,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return new ResultList<Image>
        {
            Success = images.Any(),
            Data = images,
            Message = images.Any() ? null : "No images found",
            Pagination = pagination
        };
    }
}