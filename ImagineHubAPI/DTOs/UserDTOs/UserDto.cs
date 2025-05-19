using ImagineHubAPI.DTOs.PostDTOs;
using ImagineHubAPI.Models;

namespace ImagineHubAPI.DTOs.UserDTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ProfilePicture { get; set; }
    public int PostCount { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
}