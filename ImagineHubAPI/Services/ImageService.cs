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
}