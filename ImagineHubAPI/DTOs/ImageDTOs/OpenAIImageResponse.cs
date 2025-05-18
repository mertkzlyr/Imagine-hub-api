namespace ImagineHubAPI.DTOs.ImageDTOs;

public class OpenAIImageResponse
{
    public bool Success { get; set; }
    public List<ImageData> Data { get; set; } = new();

    public class ImageData
    {
        public string Url { get; set; }
    }
}