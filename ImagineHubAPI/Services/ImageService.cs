using System.Text;
using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using ImageSharp = SixLabors.ImageSharp.Image; // Alias to avoid conflict
using ImagineHubAPI.DTOs.ImageDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class ImageService(IImageRepository imageRepository, IUserRepository userRepository, HttpClient httpClient) : IImageService
{
    public async Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId)
    {
        // Send prompt to image generation API
        var payload = new { prompt = prompt, size = "512x512" };
        var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("http://localhost:8000/api/generateimage", json);
        if (!response.IsSuccessStatusCode)
            return new Result<string> { Success = false, Message = "Failed to generate image" };
        
        // Deduct token
        var user = await userRepository.RemoveToken(userId, 1);
        if (!user)
            return new Result<string> { Success = false, Message = "Insufficient generation tokens." };

        var resultStream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<OpenAIImageResponse>(resultStream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result == null || result.Data == null || result.Data.Count == 0)
            return new Result<string> { Success = false, Message = "Invalid response from image service" };

        var imageUrl = result.Data[0].Url;
        var imageId = Guid.NewGuid();

        // Download image as bytes
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

        // Convert and save as .webp
        using var inputStream = new MemoryStream(imageBytes);
        using var image = await ImageSharp.LoadAsync(inputStream); // From ImageSharp

        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ai_pics");
        Directory.CreateDirectory(wwwrootPath);

        var fileName = $"{imageId}.webp";
        var filePath = Path.Combine(wwwrootPath, fileName);

        await image.SaveAsync(filePath, new WebpEncoder());

        // Save just the file name to the DB
        var imageEntity = new Models.Image
        {
            Id = imageId,
            UserId = userId,
            Prompt = prompt,
            ImageUrl = fileName, // just the name
            CreatedAt = DateTime.UtcNow
        };

        await imageRepository.SaveImageAsync(imageEntity);

        // Return relative URL for frontend
        return new Result<string> { Success = true, Data = $"{fileName}" };
    }

    public async Task<Result<Models.Image>> GetImageByIdAsync(Guid id, int userId)
    {
        var image = await imageRepository.GetByIdAsync(id, userId);

        if (image == null)
            return new Result<Models.Image> { Success = false, Message = "Image not found" };

        return new Result<Models.Image> { Success = true, Data = image };
    }

    public async Task<ResultList<Models.Image>> GetImagesByUserIdAsync(int userId, int page, int pageSize)
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

        return new ResultList<Models.Image>
        {
            Success = images.Any(),
            Data = images,
            Message = images.Any() ? null : "No images found",
            Pagination = pagination
        };
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await userRepository.GetByIdAsync(userId);
    }
}