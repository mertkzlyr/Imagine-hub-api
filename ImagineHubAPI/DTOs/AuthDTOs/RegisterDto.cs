namespace ImagineHubAPI.DTOs.AuthDTOs;

public class RegisterDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}