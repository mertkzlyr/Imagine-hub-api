namespace ImagineHubAPI.DTOs.PostDTOs;

public class CreatePostDto
{
    public string Description { get; set; }
    public IFormFile Picture { get; set; }
}