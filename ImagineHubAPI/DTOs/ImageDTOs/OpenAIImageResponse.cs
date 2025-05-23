using System.Text.Json.Serialization;

namespace ImagineHubAPI.DTOs.ImageDTOs;

public class OpenAIImageResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty;
}