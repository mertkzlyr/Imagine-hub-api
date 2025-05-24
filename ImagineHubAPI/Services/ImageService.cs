using System.Text;
using System.Text.Json;
using SixLabors.ImageSharp.Formats.Webp;
using ImageSharp = SixLabors.ImageSharp.Image; // Alias to avoid conflict
using ImagineHubAPI.DTOs.ImageDTOs;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImageService> _logger;

    public ImageService(
        IImageRepository imageRepository, 
        IUserRepository userRepository, 
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ImageService> logger)
    {
        _imageRepository = imageRepository;
        _userRepository = userRepository;
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result<string>> GenerateAndSaveImageAsync(string prompt, int userId)
    {
        _logger.LogInformation("GenerateAndSaveImageAsync called with prompt: {Prompt}", prompt);
        
        var aiAgentUrl = _configuration["ServiceUrls:ai-agent"];
        _logger.LogInformation("AI Agent URL from configuration: {Url}", aiAgentUrl);

        if (string.IsNullOrEmpty(aiAgentUrl))
        {
            _logger.LogError("AI Agent URL is not configured");
            return new Result<string> { Success = false, Message = "AI Agent URL is not configured" };
        }

        // Send prompt to image generation API
        var payload = new { prompt = prompt, size = "512x512" };
        var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var fullUrl = $"{aiAgentUrl.TrimEnd('/')}/api/generateimage";
        _logger.LogInformation("Sending request to: {Url}", fullUrl);
        
        var response = await _httpClient.PostAsync(fullUrl, json);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to generate image. Status code: {StatusCode}", response.StatusCode);
            return new Result<string> { Success = false, Message = "Failed to generate image" };
        }
        
        // Deduct token
        var user = await _userRepository.RemoveToken(userId, 1);
        if (!user)
            return new Result<string> { Success = false, Message = "Insufficient generation tokens." };

        var responseContent = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Raw response from AI service: {Response}", responseContent);

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = JsonSerializer.Deserialize<OpenAIImageResponse>(responseContent, options);
            _logger.LogInformation("Deserialized response: Success={Success}, Data={Data}", 
                result?.Success, result?.Data);

            if (result == null || !result.Success || string.IsNullOrEmpty(result.Data))
            {
                _logger.LogError("Invalid response from image service: Result is null or unsuccessful");
                return new Result<string> { Success = false, Message = "Invalid response from image service" };
            }

            var imageUrl = result.Data;
            _logger.LogInformation("Successfully extracted image URL: {Url}", imageUrl);

            var imageId = Guid.NewGuid();

            // Download image as bytes
            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

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

            await _imageRepository.SaveImageAsync(imageEntity);

            // Return relative URL for frontend
            return new Result<string> { Success = true, Data = $"{fileName}" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing response from image service");
            return new Result<string> { Success = false, Message = "Error deserializing response from image service" };
        }
    }

    public async Task<Result<Models.Image>> GetImageByIdAsync(Guid id, int userId)
    {
        var image = await _imageRepository.GetByIdAsync(id, userId);

        if (image == null)
            return new Result<Models.Image> { Success = false, Message = "Image not found" };

        return new Result<Models.Image> { Success = true, Data = image };
    }

    public async Task<ResultList<Models.Image>> GetImagesByUserIdAsync(int userId, int page, int pageSize)
    {
        // Fetch paged image list
        var images = await _imageRepository.GetByUserIdAsync(userId, page, pageSize);

        var totalPages = (int)Math.Ceiling(images.Count / (double)pageSize);
        var from = ((page - 1) * pageSize) + 1;
        var to = from + images.Count - 1;

        var pagination = new PaginationInformation
        {
            From = from,
            To = to,
            Total = images.Count,
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
        return await _userRepository.GetByIdAsync(userId);
    }
}